using System.ComponentModel.DataAnnotations;

namespace EventOrganizerAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHashed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public List<Attendee> Attendees { get; set; } = new List<Attendee>();
        public int RoleId { get; set; } = 1;
        public virtual Role Role { get; set; }
    }
}
