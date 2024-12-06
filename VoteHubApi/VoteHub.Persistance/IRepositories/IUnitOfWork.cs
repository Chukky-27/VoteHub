using VotingAppApi.Models;

namespace VoteHub.Persistance.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IVotingEventRepository VotingEvents { get; }
        IRepository<Candidate> Candidates { get; }
        IRepository<Vote> Votes { get; }
        Task SaveAsync();
    }
}
