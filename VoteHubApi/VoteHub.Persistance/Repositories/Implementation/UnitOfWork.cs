using VoteHub.Persistance.Repositories.Interfaces;
using VotingAppApi.Data;
using VotingAppApi.Models;

namespace VoteHub.Persistance.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VotingAppDbContext _context;
        private IVotingEventRepository _votingEvents;
        private IRepository<Candidate> _candidates;
        private IRepository<Vote> _votes;

        public UnitOfWork(VotingAppDbContext context)
        {
            _context = context;
        }

        public IVotingEventRepository VotingEvents =>
       _votingEvents ??= new VotingEventRepository(_context);

        public IRepository<Candidate> Candidates =>
            _candidates ??= new Repository<Candidate>(_context);

        public IRepository<Vote> Votes =>
            _votes ??= new Repository<Vote>(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
