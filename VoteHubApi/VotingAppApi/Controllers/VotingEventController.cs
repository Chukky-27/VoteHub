//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using VoteHub.Persistance.Services.Interfaces;
//using VoteHub.Domain.Entities;
//using VoteHub.Domain.Enums;

//namespace VoteHub.Api.Controllers
//{
//    [Route("api/voting-events")]
//    [ApiController]
//    public class VotingEventController : ControllerBase
//    {
//        private readonly IVotingEventService _eventService;
//        private readonly ILogger<VotingEventController> _logger;

//        public VotingEventController(IVotingEventService eventService, ILogger<VotingEventController> logger)
//        {
//            _eventService = eventService;
//            _logger = logger;
//        }

//        // GET: api/voting-events/cached
//        [HttpGet("cached")]
//        public async Task<IActionResult> GetCachedVotingEvents()
//        {
//            try
//            {
//                var events = await _eventService.GetUpcomingEventsAsync();
//                _logger.LogInformation("Retrieved cached upcoming events successfully.");
//                return Ok(events);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while fetching cached events.");
//                return StatusCode(500, "An error occurred while retrieving cached events.");
//            }
//        }

//        // DELETE: api/voting-events/cache
//        [HttpDelete("cache")]
//        public async Task<IActionResult> ClearCache()
//        {
//            try
//            {
//                await _eventService.ClearVotingEventsCacheAsync();
//                _logger.LogInformation("Voting events cache cleared successfully.");
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while clearing the cache.");
//                return StatusCode(500, "An error occurred while clearing the cache.");
//            }
//        }

//        // POST: api/voting-events/create
//        [Authorize(Roles = "Admin")]
//        [HttpPost("create")]
//        public async Task<IActionResult> CreateEvent([FromBody] VotingEvent votingEvent)
//        {
//            if (!ModelState.IsValid)
//            {
//                _logger.LogWarning("CreateEvent request received with invalid model state.");
//                return BadRequest(ModelState);
//            }

//            if (votingEvent.StartDate >= votingEvent.EndDate)
//            {
//                _logger.LogWarning("Invalid date range: StartDate ({StartDate}) >= EndDate ({EndDate}).", 
//                                    votingEvent.StartDate, votingEvent.EndDate);
//                return BadRequest("End date must be later than the start date.");
//            }

//            try
//            {
//                // Check for existing event with the same name
//                var existingEvent = await _eventService.GetEventByNameAsync(votingEvent.Name);
//                if (existingEvent != null)
//                {
//                    _logger.LogInformation("An event with the name '{EventName}' already exists.", votingEvent.Name);
//                    return Conflict($"An event with the name '{votingEvent.Name}' already exists.");
//                }

//                // Check for overlapping event
//                var overlappingEvent = await _eventService.GetOverlappingEventAsync(
//                    votingEvent.StartDate.Value, votingEvent.EndDate.Value);
//                if (overlappingEvent != null)
//                {
//                    _logger.LogInformation("The event dates overlap with an existing event: '{EventName}' (ID: {EventId}).", 
//                                           overlappingEvent.Name, overlappingEvent.Id);
//                    return Conflict("The event dates overlap with an existing event.");
//                }

//                // Create the new event
//                var newEvent = new VotingEvent
//                {
//                    Name = votingEvent.Name,
//                    Status = VotingStatus.Upcoming,
//                    StartDate = votingEvent.StartDate,
//                    EndDate = votingEvent.EndDate
//                };

//                await _eventService.CreateEventAsync(newEvent);
//                _logger.LogInformation("Event '{EventName}' created successfully with ID: {EventId}.", 
//                                       newEvent.Name, newEvent.Id);

//                // Return the created event
//                return CreatedAtAction(nameof(GetEventById), new { id = newEvent.Id }, newEvent);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Unexpected error occurred while creating a voting event.");
//                return StatusCode(500, "Internal server error occurred. Please try again later.");
//            }
//        }

//        // GET: api/voting-events/{id}
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetEventById(int id)
//        {
//            try
//            {
//                var votingEvent = await _eventService.GetVotingEventByIdAsync(id);
//                if (votingEvent == null)
//                {
//                    _logger.LogWarning("Voting event with ID {EventId} not found.", id);
//                    return NotFound($"Voting event with ID {id} not found.");
//                }

//                _logger.LogInformation("Voting event with ID {EventId} retrieved successfully.", id);
//                return Ok(votingEvent);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while retrieving the voting event with ID {EventId}.", id);
//                return StatusCode(500, "An error occurred while retrieving the event.");
//            }
//        }
//    }
//}
