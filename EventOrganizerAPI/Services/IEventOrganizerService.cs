using EventOrganizerAPI.Entities;
using EventOrganizerAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EventOrganizerAPI.Services
{
    public interface IEventOrganizerService
    {
        Task DeleteById(int id);
        Task<IEnumerable<EventDto>> GetAll();
        Task<EventDto> GetById(int id);
        Task<int> CreateEvent(CreateEventDto dto);
        Task UpdateEvent(UpdateEventDto dto, int id);
        Task Join(int eventId);
        Task Leave(int eventId);
    }
}