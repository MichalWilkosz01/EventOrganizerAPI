using EventOrganizerAPI.Models.Dto;

namespace EventOrganizerAPI.Services
{
    public interface IAccountService
    {
        Task<string> GenerateJWT(LoginUserDto dto);
        Task RegisterUser(RegisterUserDto dto);
    }
}