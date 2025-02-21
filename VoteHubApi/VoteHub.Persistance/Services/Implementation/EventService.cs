using Microsoft.Extensions.Logging;
using VoteHub.Domain.Entities;
using VoteHub.Persistance.Repositories.Interfaces;
using VoteHub.Persistance.Services.Interfaces;

namespace VoteHub.Persistance.Services.Implementation
{
    public class EventService(IVotingEventRepository eventRepository, ILogger<EventService> logger) : IEventService
    {
        private readonly IVotingEventRepository _eventRepository = eventRepository;
        private readonly ILogger<EventService> _logger = logger;

        public async Task<VotingEvent?> GetEventByNameAsync(string eventName)
        {
            _logger.LogInformation("Checking if event with name '{EventName}' exists.", eventName);
            return await _eventRepository.GetEventByNameAsync(eventName);
        }

        public async Task<VotingEvent?> GetOverlappingEventAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Checking for overlapping events between '{StartDate}' and '{EndDate}'.", startDate, endDate);
            return await _eventRepository.GetOverlappingEventAsync(startDate, endDate);
        }

        public async Task CreateEventAsync(VotingEvent votingEvent)
        {
            try
            {
                _logger.LogInformation("Creating event with name '{EventName}'.", votingEvent.Name);
                await _eventRepository.AddAsync(votingEvent);
                _logger.LogInformation("Event with name '{EventName}' created successfully.", votingEvent.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating event with name '{EventName}'.", votingEvent.Name);
                throw;  // Rethrow the exception to be handled by the controller
            }
        }
    }

}
