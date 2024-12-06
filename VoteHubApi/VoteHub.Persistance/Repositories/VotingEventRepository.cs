using Microsoft.EntityFrameworkCore;
using VoteHub.Domain.Entities;
using VoteHub.Persistance.IRepositories;
using VotingAppApi.Data;

namespace VoteHub.Persistance.Repositories
{
    public class VotingEventRepository : Repository<VotingEvent>, IVotingEventRepository
    {
        private readonly VotingAppDbContext _context;

        public VotingEventRepository(VotingAppDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<VotingEvent>> GetUpcomingEventsAsync()
        {
            return await _context.VotingEvents
                .Where(e => e.StartDate > DateTime.UtcNow)
                .ToListAsync();
        }
    }
}
