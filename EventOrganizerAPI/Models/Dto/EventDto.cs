using EventOrganizerAPI.Entities;

namespace EventOrganizerAPI.Models.Dto
{
    public class EventDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int NumberOfParticipants { get; set; }
        public DateTime EventCreatedDate { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public List<Attendee> Attendees { get; set; } = new List<Attendee>();
    }
}
