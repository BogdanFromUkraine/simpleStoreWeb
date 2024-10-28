using Authorization.Services;

namespace Authorization.Extensions
{
    public static class Extensions
    {
        public static IEndpointConventionBuilder RequirePermissions<TBuilder>(
            this TBuilder builder, params Enum.Permission[] permissions)
            where TBuilder : IEndpointConventionBuilder
        {
            return builder.RequireAuthorization(policy =>
            {
                policy.AddRequirements(new PermissionRequirement(permissions));
            });
        }
    }
}