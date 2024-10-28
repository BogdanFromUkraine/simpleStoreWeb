namespace Authorization.Services
{
    public interface IPermissionService
    {
        public Task<HashSet<Authorization.Enum.Permission>> GetPermissionsAsync(Guid userId);
    }
}