using System.Security.Claims;

namespace EventOrganizerAPI.Services
{
    public interface IUserContextService
    {
        ClaimsPrincipal? User { get; }

        int? GetUserId();
    }
}