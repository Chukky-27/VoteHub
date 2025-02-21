using VoteHub.Domain.Entities;
using VotingAppApi.Models;

namespace VoteHub.Persistance.Services.Interfaces
{
    public interface IVotingEventService
    {
        Task<IEnumerable<VotingEvent>> GetUpcomingEventsAsync();
        Task CreateVotingEventAsync(VotingEvent votingEvent);
        Task SaveUserVoteAsync(int userId, Vote vote);
        Task<Vote?> GetUserVoteAsync(int userId);
        Task AddCandidateToEventAsync(int eventId, Candidate candidate);
        Task AddVoteToEventAsync(int eventId, Vote vote);
        Task<int> GetTotalVotesForEventAsync(int eventId);
        Task UpdateEventStatusAsync(int eventId);
        Task<bool> CanVoteAsync(int eventId, int voterId);

        Task ClearVotingEventsCacheAsync();
        Task<VotingEvent?> GetEventByNameAsync(string eventName);
        Task<VotingEvent?> GetOverlappingEventAsync(DateTime startDate, DateTime endDate);
        Task<VotingEvent?> GetVotingEventByIdAsync(int id);
    }
}
