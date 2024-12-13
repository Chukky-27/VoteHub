using Microsoft.EntityFrameworkCore;
using VoteHub.Domain.Entities;
using VoteHub.Persistance.Repositories.Interfaces;
using VotingAppApi.Data;

namespace VoteHub.Persistance.Repositories.Implementation
{
    public class EventRepository : IEventRepository
    {
        private readonly VotingAppDbContext _context;

        public EventRepository(VotingAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VotingEvent>> GetUpcomingEventsAsync()
        {
            // Assuming VotingEvent has a StartDate property
            return await _context.VotingEvents
                                 .Where(e => e.StartDate > DateTime.Now) // Filter for upcoming events
                                 .ToListAsync();
        }

        public async Task<VotingEvent?> GetEventByNameAsync(string eventName)
        {
            return await _context.VotingEvents
                                 .FirstOrDefaultAsync(e => e.Name == eventName);
        }

        public async Task<VotingEvent?> GetOverlappingEventAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.VotingEvents
                                 .FirstOrDefaultAsync(e => e.StartDate < endDate && e.EndDate > startDate);

        }

        public async Task AddAsync(VotingEvent votingEvent)
        {
            await _context.VotingEvents.AddAsync(votingEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<VotingEvent?> GetByIdAsync(int eventId)
        {
         return  await _context.VotingEvents
                                    .FirstOrDefaultAsync(e => e.Id == eventId);
            //if (votingEvent == null)
            //{
            //    throw new KeyNotFoundException($"VotingEvent with ID {eventId} not found.");
            //}
            //return votingEvent;
        }
    }
}
