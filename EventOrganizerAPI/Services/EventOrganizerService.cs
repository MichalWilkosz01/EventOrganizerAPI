using AutoMapper;
using EventOrganizerAPI.Authorization;
using EventOrganizerAPI.Entities;
using EventOrganizerAPI.Exceptions;
using EventOrganizerAPI.Models.Dto;
using EventOrganizerAPI.Persistance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Transactions;

namespace EventOrganizerAPI.Services
{
    public class EventOrganizerService : IEventOrganizerService
    {
        private readonly EventOrganizerDbContext _dbContext;
        private readonly ILogger<EventOrganizerService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;
        private readonly IAuthorizationService _authorizationService;

        public EventOrganizerService(EventOrganizerDbContext dbContext, ILogger<EventOrganizerService> logger, IMapper mapper, 
            IUserContextService userContext, IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _userContext = userContext;
            _authorizationService = authorizationService;
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

            var eventById = await _dbContext.Events
                .Include(e => e.Location)
                .FirstOrDefaultAsync(e => e.Id == id) 
                ?? throw new NotFoundException("Event not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContext.User, eventById,
                new PermissionRequirement(AccessOperation.Delete)).Result;

            if(!authorizationResult.Succeeded) 
            {
                throw new PermissionDeniedException("Access denied: You do not have permission to delete this event");
            }
           
            var locationToDelete = eventById.Location;

            _dbContext.Locations.Remove(locationToDelete);

            _dbContext.Events.Remove(eventById);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateEvent(CreateEventDto dto)
        {
            var newEvent = _mapper.Map<Event>(dto);

            var userId = int.Parse(_userContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            newEvent.OrganizerId = userId;

            await _dbContext.Events.AddAsync(newEvent);
            await _dbContext.SaveChangesAsync();

            Attendee attendee = new Attendee()
            {
                UserId = userId,
                EventId = newEvent.Id
            };

            await _dbContext.Attendees.AddAsync(attendee);
            await _dbContext.SaveChangesAsync();

            return newEvent.Id;
        }

        public async Task UpdateEvent(UpdateEventDto dto, int id)
        {
            var eventToUpdate = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == id) 
                ?? throw new NotFoundException("Event not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContext.User, eventToUpdate,
                new PermissionRequirement(AccessOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new PermissionDeniedException("Access denied: You do not have permission to update this event");
            }

            if (dto.EventStartDate < dto.EventEndDate)
            {
                throw new InvalidDateRangeException("Start date cannot be later than end date");
            }

            if(dto.EventEndDate < DateTime.Today || dto.EventStartDate < DateTime.Today)
            {
                throw new InvalidDateRangeException("Dates cannot be in the past");
            }

            _mapper.Map(dto, eventToUpdate);

            _dbContext.Update(eventToUpdate);
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task Join(int eventId)
        {
            var eventToJoin = await _dbContext.Events
                .FirstOrDefaultAsync(e => e.Id == eventId) 
                ?? throw new NotFoundException("Event not found");

            var userId = int.Parse(_userContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var isUserParticipant = _dbContext.Attendees.Any(a => a.UserId == userId && a.EventId == eventId);

            if (isUserParticipant)
            {
                throw new UserAlreadyParticipatingException("The user is already a participant in this event");
            }

            eventToJoin.NumberOfParticipants += 1;
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            Attendee attendee = new Attendee()
            {
                UserId = userId,
                EventId = eventToJoin.Id
                
            };

            await _dbContext.Attendees.AddAsync(attendee);
            await _dbContext.SaveChangesAsync();
        }

    }
}
