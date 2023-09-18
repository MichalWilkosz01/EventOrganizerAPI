using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

namespace EventOrganizerAPI.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public int? GetUserId()
        {
            var nameIdentifierClaim = User?.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

            if (nameIdentifierClaim != null && int.TryParse(nameIdentifierClaim.Value, out int userId))
            {
                return userId;
            }

            return null;
        }

    }
}
