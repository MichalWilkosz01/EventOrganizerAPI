namespace EventOrganizerAPI.Entities
{
    public class Attendee
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual Event Event { get; set; }
        public int EventId { get; set; }
    }
}
