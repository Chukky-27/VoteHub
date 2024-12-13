using VotingAppApi.Models;

namespace VoteHub.Persistance.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IVotingEventRepository VotingEvents { get; }
        IRepository<Candidate> Candidates { get; }
        IRepository<Vote> Votes { get; }
        Task SaveAsync();
    }
}
