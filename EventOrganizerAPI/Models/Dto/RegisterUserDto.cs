using EventOrganizerAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace EventOrganizerAPI.Models.Dto
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
    }
}
