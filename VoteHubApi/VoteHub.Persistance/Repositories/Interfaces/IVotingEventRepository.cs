using VoteHub.Domain.Entities;

namespace VoteHub.Persistance.Repositories.Interfaces
{
    public interface IVotingEventRepository : IRepository<VotingEvent>
    {
        Task<IEnumerable<VotingEvent>> GetUpcomingEventsAsync();
    }
}
