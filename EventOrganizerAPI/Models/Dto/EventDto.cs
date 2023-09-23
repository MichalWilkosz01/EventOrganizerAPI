using EventOrganizerAPI.Entities;
using EventOrganizerAPI.JsonConverters;
using Newtonsoft.Json;

namespace EventOrganizerAPI.Models.Dto
{
    public class EventDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int NumberOfParticipants { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EventCreatedDate { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EventStartDate { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime EventEndDate { get; set; }
        public List<Attendee> Attendees { get; set; } = new List<Attendee>();
    }
}
