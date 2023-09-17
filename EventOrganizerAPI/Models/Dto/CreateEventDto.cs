using EventOrganizerAPI.Entities;

namespace EventOrganizerAPI.Models.Dto
{
    public class CreateEventDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
    }
}
