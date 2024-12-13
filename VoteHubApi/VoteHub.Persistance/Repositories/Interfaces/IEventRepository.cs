using VoteHub.Domain.Entities;

namespace VoteHub.Persistance.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<VotingEvent>> GetUpcomingEventsAsync();  // Get events that are in the future
        Task<VotingEvent?> GetEventByNameAsync(string eventName);  // Get event by name
        Task<VotingEvent?> GetOverlappingEventAsync(DateTime startDate, DateTime endDate);  // Check for overlapping events
        Task AddAsync(VotingEvent votingEvent);  // Add a new event
        Task<VotingEvent?> GetByIdAsync(int eventId);  // Get event by ID
    }
}
