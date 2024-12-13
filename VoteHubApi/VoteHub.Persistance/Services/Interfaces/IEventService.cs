using VoteHub.Domain.Entities;

namespace VoteHub.Persistance.Services.Interfaces
{
    public interface IEventService
    {
        /// <summary>
        /// Retrieves a voting event by its name.
        /// </summary>
        /// <param name="eventName">The name of the voting event.</param>
        /// <returns>The matching VotingEvent or null if not found.</returns>
        Task<VotingEvent?> GetEventByNameAsync(string eventName);

        /// <summary>
        /// Checks for an overlapping event within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>The overlapping VotingEvent or null if none exist.</returns>
        Task<VotingEvent?> GetOverlappingEventAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Creates a new voting event.
        /// </summary>
        /// <param name="votingEvent">The VotingEvent to be created.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task CreateEventAsync(VotingEvent votingEvent);
    }
}
