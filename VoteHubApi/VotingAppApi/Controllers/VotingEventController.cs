using Microsoft.AspNetCore.Mvc;
using VoteHub.Domain.Entities;
using VoteHub.Persistance.IServices;
using VoteHub.Persistance.Services;

namespace VoteHub.Api.Controllers
{
    [Route("api/voting-events")]
    [ApiController]
    public class VotingEventController : ControllerBase
    {
        private readonly IDistributedCacheService _cacheService;

        public VotingEventController(IDistributedCacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("cached")]
        public async Task<IActionResult> GetCachedVotingEvents()
        {
            var events = await _cacheService.GetCachedVotingEventsAsync();
            return Ok(events);
        }

        [HttpDelete("cache")]
        public async Task<IActionResult> ClearCache()
        {
            await _cacheService.ClearVotingEventsCacheAsync();
            return NoContent();
        }
    }


}
