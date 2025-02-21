using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using VoteHub.Domain.Entities;
using VoteHub.Domain.Enums;
using VoteHub.Persistance.Repositories.Interfaces;
using VoteHub.Persistance.Services.Interfaces;
using VotingAppApi.Models;

namespace VoteHub.Persistance.Services.Implementation
{
    public class VotingEventService : IVotingEventService
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

        public async Task<VotingEvent?> GetVotingEventByIdAsync(int eventId)
        {
            var votingEvent = await _unitOfWork.VotingEvents.GetByIdAsync(eventId);
            return votingEvent ?? throw new ArgumentException($"VotingEvent with ID {eventId} not found.");
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

        public async Task SaveUserVoteAsync(int userId, Vote vote)
        {
            var cacheKey = $"UserVote_{userId}";
            var serializedVote = JsonSerializer.Serialize(vote);

            await _redisCache.SetStringAsync(cacheKey, serializedVote, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
        }

        public async Task<Vote?> GetUserVoteAsync(int userId)
        {
            var cacheKey = $"UserVote_{userId}";
            var cachedVote = await _redisCache.GetStringAsync(cacheKey);

            return cachedVote != null ? JsonSerializer.Deserialize<Vote>(cachedVote) : null;
        }

        public async Task<VotingEvent?> GetEventByNameAsync(string eventName)
        {
            var votingEvent = await _unitOfWork.VotingEvents.GetEventByNameAsync(eventName);
            return votingEvent ?? throw new ArgumentException($"VotingEvent with name '{eventName}' not found.");
        }

        public async Task<VotingEvent?> GetOverlappingEventAsync(DateTime startDate, DateTime endDate)
        {
            var votingEvent = await _unitOfWork.VotingEvents.GetOverlappingEventAsync(startDate, endDate);
            return votingEvent ?? throw new ArgumentException("Overlapping VotingEvent not found.");
        }

        public async Task AddCandidateToEventAsync(int eventId, Candidate candidate)
        {
            var votingEvent = await GetVotingEventByIdAsync(eventId);
            votingEvent.Candidates ??= new List<Candidate>();
            votingEvent.Candidates.Add(candidate);

            await _unitOfWork.SaveAsync();
            _memoryCache.Remove(UpcomingEventsCacheKey);
        }

        public async Task AddVoteToEventAsync(int eventId, Vote vote)
        {
            var votingEvent = await GetVotingEventByIdAsync(eventId);
            votingEvent.Votes ??= new List<Vote>();
            votingEvent.Votes.Add(vote);

            await _unitOfWork.SaveAsync();
            _memoryCache.Remove(UpcomingEventsCacheKey);
        }

        public async Task<int> GetTotalVotesForEventAsync(int eventId)
        {
            var votingEvent = await GetVotingEventByIdAsync(eventId);
            return votingEvent.Votes?.Count ?? 0;
        }

        public async Task UpdateEventStatusAsync(int eventId)
        {
            var votingEvent = await GetVotingEventByIdAsync(eventId);
            var now = DateTime.UtcNow;

            if (votingEvent.StartDate <= now && votingEvent.EndDate >= now)
            {
                votingEvent.Status = VotingStatus.Ongoing;
            }
            else if (votingEvent.EndDate < now)
            {
                votingEvent.Status = VotingStatus.Completed;
            }
            else
            {
                votingEvent.Status = VotingStatus.Upcoming;
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> CanVoteAsync(int eventId, int voterId)
        {
            var votingEvent = await GetVotingEventByIdAsync(eventId);
            if (votingEvent.Status != VotingStatus.Ongoing)
                return false;

            // Check if the user has already voted
            var alreadyVoted = votingEvent.Votes?.Any(v => v.UserId == voterId) ?? false;
            return !alreadyVoted;
        }

         // Cache clear method
        public async Task ClearVotingEventsCacheAsync()
        {
            _memoryCache.Remove(UpcomingEventsCacheKey);
        }

        //Task<VotingEvent?> GetVotingEventByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
         
    
}
