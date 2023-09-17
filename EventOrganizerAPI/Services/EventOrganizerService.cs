using AutoMapper;
using EventOrganizerAPI.Entities;
using EventOrganizerAPI.Exceptions;
using EventOrganizerAPI.Models.Dto;
using EventOrganizerAPI.Persistance;
using Microsoft.EntityFrameworkCore;

namespace EventOrganizerAPI.Services
{
    public class EventOrganizerService : IEventOrganizerService
    {
        private readonly EventOrganizerDbContext _dbContext;
        private readonly ILogger<EventOrganizerService> _logger;
        private readonly IMapper _mapper;

        public EventOrganizerService(EventOrganizerDbContext dbContext, ILogger<EventOrganizerService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>> GetAll()
        {
            var events = await _dbContext.Events
                .Include(e => e.Location)
                .ToListAsync();

            var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);

            return eventsDto;
        }

        public async Task<EventDto> GetById(int id)
        {
            var eventById = await _dbContext.Events
                .Include(e => e.Location)
                .SingleOrDefaultAsync(e => e.Id == id)
                ?? throw new NotFoundException("Event not found");

            var eventByIdDto = _mapper.Map<EventDto>(eventById);

            return eventByIdDto;
        }

        public async Task DeleteById(int id)
        {
            _logger.LogInformation($"Event with id: {id} DELETE action invoked");

            var eventById = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id) 
                ?? throw new NotFoundException("Event not found");

            _dbContext.Events.Remove(eventById);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateEvent(CreateEventDto dto)
        {
            var newEvent = _mapper.Map<Event>(dto);

            //Temporary user
            newEvent.OrganizerId = 1;

            await _dbContext.Events.AddAsync(newEvent);
            await _dbContext.SaveChangesAsync();

            return newEvent.Id;
        }

        public async Task UpdateEvent(UpdateEventDto dto, int id)
        {
            var eventToUpdate = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == id) 
                ?? throw new NotFoundException("Event not found");
            
            if (dto.EventStartDate < dto.EventEndDate)
            {
                throw new InvalidDateRangeException("Start date cannot be later than end date");
            }

            _mapper.Map(dto, eventToUpdate);

            _dbContext.Update(eventToUpdate);
            
            await _dbContext.SaveChangesAsync();
        }
    }
}
