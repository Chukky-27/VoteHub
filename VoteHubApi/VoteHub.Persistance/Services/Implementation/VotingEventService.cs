using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using VoteHub.Domain.Entities;
using VoteHub.Persistance.Repositories.Interfaces;
using VotingAppApi.Models;

namespace VoteHub.Persistance.Services.Implementation
{
    public class VotingEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _redisCache;

        private const string UpcomingEventsCacheKey = "UpcomingVotingEvents";

        public VotingEventService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, IDistributedCache redisCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _redisCache = redisCache;
        }

        public async Task<IEnumerable<VotingEvent>> GetUpcomingEventsAsync()
        {
            if (_memoryCache.TryGetValue(UpcomingEventsCacheKey, out IEnumerable<VotingEvent> cachedEvents))
            {
                return cachedEvents;
            }

            var upcomingEvents = await _unitOfWork.VotingEvents.GetUpcomingEventsAsync();
            _memoryCache.Set(UpcomingEventsCacheKey, upcomingEvents, TimeSpan.FromMinutes(10));

            return upcomingEvents;
        }

        public async Task CreateVotingEventAsync(VotingEvent votingEvent)
        {
            await _unitOfWork.VotingEvents.AddAsync(votingEvent);
            await _unitOfWork.SaveAsync();
            _memoryCache.Remove(UpcomingEventsCacheKey);
        }

        // New method for saving a user vote in Redis
        public async Task SaveUserVoteAsync(int userId, Vote vote)
        {
            var cacheKey = $"UserVote_{userId}";
            var serializedVote = JsonSerializer.Serialize(vote);

            // Store the vote in Redis with a 30-minute expiration
            await _redisCache.SetStringAsync(cacheKey, serializedVote, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
        }

        // New method for retrieving a user vote from Redis
        public async Task<Vote> GetUserVoteAsync(int userId)
        {
            var cacheKey = $"UserVote_{userId}";
            var cachedVote = await _redisCache.GetStringAsync(cacheKey);

            if (cachedVote != null)
            {
                return JsonSerializer.Deserialize<Vote>(cachedVote);
            }

            return null; // Return null if no cached vote exists
        }

    }
}
