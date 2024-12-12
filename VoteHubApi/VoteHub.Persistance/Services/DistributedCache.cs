using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using VoteHub.Domain.Entities;
using VoteHub.Persistance.IRepositories;
using VoteHub.Persistance.IServices;

namespace VoteHub.Persistance.Services
{
    public class DistributedCacheService : IDistributedCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IUnitOfWork _unitOfWork;

        public DistributedCacheService(IDistributedCache cache, IUnitOfWork unitOfWork)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VotingEvent>> GetCachedVotingEventsAsync()
        {
            const string cacheKey = "VotingEvents";
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<IEnumerable<VotingEvent>>(cachedData);
            }

            var votingEvents = await _unitOfWork.VotingEvents.GetAllAsync();
            var serializedData = JsonSerializer.Serialize(votingEvents);

            await _cache.SetStringAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            return votingEvents;
        }

        public async Task ClearVotingEventsCacheAsync()
        {
            const string cacheKey = "VotingEvents";
            await _cache.RemoveAsync(cacheKey);
        }
    }

}


