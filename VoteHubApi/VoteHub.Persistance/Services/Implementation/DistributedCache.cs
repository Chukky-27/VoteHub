using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using VoteHub.Domain.Entities;
using VoteHub.Persistance.Repositories.Interfaces;
using VoteHub.Persistance.Services.Interfaces;

namespace VoteHub.Persistance.Services.Implementation
{
    public class DistributedCacheService : IDistributedCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IUnitOfWork _unitOfWork;

        private const string VotingEventsCacheKey = "VotingEvents";
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true, // Ensures proper deserialization regardless of case
            WriteIndented = false               // Reduces cache size by avoiding unnecessary formatting
        };

        public DistributedCacheService(IDistributedCache cache, IUnitOfWork unitOfWork)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<VotingEvent>> GetCachedVotingEventsAsync()
        {
            try
            {
                // Check for cached data
                var cachedData = await _cache.GetStringAsync(VotingEventsCacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    return JsonSerializer.Deserialize<IEnumerable<VotingEvent>>(cachedData, JsonOptions)
                           ?? Enumerable.Empty<VotingEvent>();
                }

                // Fetch from database if cache is empty
                var votingEvents = await _unitOfWork.VotingEvents.GetAllAsync();
                if (votingEvents == null)
                {
                    return Enumerable.Empty<VotingEvent>();
                }

                // Cache the data
                var serializedData = JsonSerializer.Serialize(votingEvents, JsonOptions);
                await _cache.SetStringAsync(VotingEventsCacheKey, serializedData, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache expires in 30 minutes
                });

                return votingEvents;
            }
            catch (Exception ex)
            {
                // Log exception (implement a logging service if available)
                Console.WriteLine($"Error fetching or caching VotingEvents: {ex.Message}");
                return Enumerable.Empty<VotingEvent>();
            }
        }

        public async Task ClearVotingEventsCacheAsync()
        {
            try
            {
                await _cache.RemoveAsync(VotingEventsCacheKey);
            }
            catch (Exception ex)
            {
                // Log exception
                Console.WriteLine($"Error clearing VotingEvents cache: {ex.Message}");
            }
        }
    }
}
