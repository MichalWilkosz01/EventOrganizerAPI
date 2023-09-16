using EventOrganizerAPI.Models;

namespace EventOrganizerAPI.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrganizerId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public virtual User Organizer { get; set; }
        public int NumberOfParticipants { get; set; } = 0;
        public DateTime EventCreatedDate { get; set; } = DateTime.Now;
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public List<Attendee> Attendees { get; set; } = new List<Attendee>();
    }
}
