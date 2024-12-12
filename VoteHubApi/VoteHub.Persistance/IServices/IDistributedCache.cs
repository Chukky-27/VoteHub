using VoteHub.Domain.Entities;

namespace VoteHub.Persistance.IServices
{
    public interface IDistributedCacheService
    {
        /// <summary>
        /// Gets cached voting events. If not cached, retrieves them from the database and caches the results.
        /// </summary>
        /// <returns>A list of VotingEvent objects.</returns>
        Task<IEnumerable<VotingEvent>> GetCachedVotingEventsAsync();

        /// <summary>
        /// Clears the cache for voting events.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ClearVotingEventsCacheAsync();
    }

}
