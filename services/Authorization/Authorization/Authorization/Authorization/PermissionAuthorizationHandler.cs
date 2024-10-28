using Microsoft.AspNetCore.Authorization;

namespace Authorization.Services
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceProviderFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceProviderFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(
                c => c.Type == "userId");

            if (userId is null || !Guid.TryParse(userId.Value, out var id))
            {
                throw new ArgumentException("Invalid or missing userId claim.");
            }

            using var scope = _serviceProviderFactory.CreateScope();

            var permissionService = scope.ServiceProvider
                .GetService<IPermissionService>();

            var permissions = await permissionService.GetPermissionsAsync(id);

            if (permissions.Intersect(requirement.Permissions).Any())
            {
                context.Succeed(requirement);
            }
        }
    }
}