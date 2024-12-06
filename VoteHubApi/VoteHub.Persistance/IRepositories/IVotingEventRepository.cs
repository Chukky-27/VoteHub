using VoteHub.Domain.Entities;

namespace VoteHub.Persistance.IRepositories
{
    public interface IVotingEventRepository : IRepository<VotingEvent>
    {
        Task<IEnumerable<VotingEvent>> GetUpcomingEventsAsync();
    }
}
