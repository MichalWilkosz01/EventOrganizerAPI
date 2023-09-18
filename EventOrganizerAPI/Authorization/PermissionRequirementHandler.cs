using EventOrganizerAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EventOrganizerAPI.Authorization
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement, Event>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement, 
            Event resource)
        {
            if (requirement.AccessOperation == AccessOperation.Read
                || requirement.AccessOperation == AccessOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (resource.OrganizerId == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
