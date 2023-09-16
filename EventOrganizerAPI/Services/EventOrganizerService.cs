using EventOrganizerAPI.Entities;
using EventOrganizerAPI.Exceptions;
using EventOrganizerAPI.Persistance;
using Microsoft.EntityFrameworkCore;

namespace EventOrganizerAPI.Services
{
    public class EventOrganizerService : IEventOrganizerService
    {
        private readonly EventOrganizerDbContext _dbContext;
        private readonly ILogger<EventOrganizerService> _logger;

        public EventOrganizerService(EventOrganizerDbContext dbContext, ILogger<EventOrganizerService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            var events = await _dbContext.Events.ToListAsync();
            return events;
        }

        public async Task<Event> GetById(int id)
        {
            var eventById = await _dbContext.Events.SingleOrDefaultAsync(e => e.Id == id);

            if (eventById == null)
            {
                throw new NotFoundException("Event not found");
            }

            return eventById;
        }
    }
}
