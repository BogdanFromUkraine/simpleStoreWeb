

namespace Authorization.Services
{
    using Repository.IRepository;
    public class PermissionService : IPermissionService
    {
        private readonly IUserRepository _userRepository;

        public PermissionService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<HashSet<Authorization.Enum.Permission>> GetPermissionsAsync(Guid userId)
        {
            return _userRepository.GetUserPermission(userId);
        }
    }
}