using EventOrganizerAPI.Entities;

namespace EventOrganizerAPI.Services
{
    public interface IEventOrganizerService
    {
        Task<IEnumerable<Event>> GetAll();
        Task<Event> GetById(int id);
    }
}