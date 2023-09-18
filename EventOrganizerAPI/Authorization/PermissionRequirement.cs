using Microsoft.AspNetCore.Authorization;

namespace EventOrganizerAPI.Authorization
{
    public enum AccessOperation
    {
        Create,
        Read,
        Update,
        Delete
    }

    public class PermissionRequirement : IAuthorizationRequirement
    {
        public AccessOperation AccessOperation { get; set; }

        public PermissionRequirement(AccessOperation accessOperation)
        {
            AccessOperation = accessOperation;
        }
    }
}
