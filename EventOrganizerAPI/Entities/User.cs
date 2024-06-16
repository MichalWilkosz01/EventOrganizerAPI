using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public int RoleId { get; set; } 
        public virtual Role Role { get; set; }
        public List<Event> AttendingEvents { get; } = new List<Event>();
        public List<Event> OrganizedEvents { get; } = new List<Event>();
    }
}
