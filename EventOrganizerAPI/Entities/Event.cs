using EventOrganizerAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventOrganizerAPI.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual User Organizer { get; set; }
        public int OrganizerId { get; set; }
        public virtual Location Location { get; set; }
        public int LocationId { get; set; }
        public int NumberOfParticipants { get; set; } = 0;
        public DateTime EventCreatedDate { get; set; } = DateTime.Now;
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public List<User> Attendees { get; } = new List<User>();
    }
}
