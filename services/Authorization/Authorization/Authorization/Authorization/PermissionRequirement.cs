using Microsoft.AspNetCore.Authorization;

namespace Authorization.Services
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(Enum.Permission[] permissions)
        {
            Permissions = permissions;
        }

        public Enum.Permission[] Permissions { get; set; } = [];
    }
}