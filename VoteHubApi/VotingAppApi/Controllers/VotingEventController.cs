using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoteHub.Persistance.Services.Interfaces;
using VoteHub.Domain.Entities;
using VoteHub.Domain.Enums;

namespace VoteHub.Api.Controllers
{
    [Route("api/voting-events")]
    [ApiController]
    public class VotingEventController(
        IDistributedCacheService cacheService,
        IEventService eventService,
        ILogger<VotingEventController> logger) : ControllerBase
    {
        private readonly IDistributedCacheService _cacheService = cacheService;
        private readonly IEventService _eventService = eventService;
        private readonly ILogger<VotingEventController> _logger = logger;

        // GET: api/voting-events/cached
        [HttpGet("cached")]
        public async Task<IActionResult> GetCachedVotingEvents()
        {
            var events = await _cacheService.GetCachedVotingEventsAsync();
            return Ok(events);
        }

        // DELETE: api/voting-events/cache
        [HttpDelete("cache")]
        public async Task<IActionResult> ClearCache()
        {
            await _cacheService.ClearVotingEventsCacheAsync();
            return NoContent();
        }

        // POST: api/voting-events/create-event
        [Authorize(Roles = "Admin")]
        [HttpPost("create-event")]
        public async Task<IActionResult> CreateEvent([FromBody] VotingEvent votingEvent)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateEvent request received with invalid data.");
                return BadRequest(ModelState);
            }

            if (votingEvent.StartDate >= votingEvent.EndDate)
            {
                _logger.LogWarning("Invalid date range: StartDate ({StartDate}) >= EndDate ({EndDate}).", votingEvent.StartDate, votingEvent.EndDate);
                return BadRequest("End date must be later than the start date.");
            }

            try
            {
                var existingEvent = await _eventService.GetEventByNameAsync(votingEvent.Name);
                if (existingEvent != null)
                {
                    _logger.LogInformation("An event with the name '{EventName}' already exists.", votingEvent.Name);
                    return Conflict("An event with the same name already exists.");
                }

                var overlappingEvent = await _eventService.GetOverlappingEventAsync(votingEvent.StartDate.Value, votingEvent.EndDate.Value);
                if (overlappingEvent != null)
                {
                    _logger.LogInformation("The event dates overlap with an existing event.");
                    return Conflict("The event dates overlap with an existing event.");
                }

                var newEvent = new VotingEvent
                {
                    Name = votingEvent.Name,
                    Status = VotingStatus.Upcoming,
                    StartDate = votingEvent.StartDate,
                    EndDate = votingEvent.EndDate
                };

                await _eventService.CreateEventAsync(newEvent);
                _logger.LogInformation("Event '{EventName}' successfully created with ID: {EventId}.", newEvent.Name, newEvent.Id);

                return CreatedAtAction(nameof(CreateEvent), new { id = newEvent.Id }, newEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating event.");
                return StatusCode(500, "Internal server error occurred. Please try again later.");
            }
        }
    }
}
