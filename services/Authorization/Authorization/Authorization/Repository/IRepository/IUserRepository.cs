using Authorization.Models;
using Notes_project.Models.ModelsDTO;
using System.Linq.Expressions;
using ProductService.Models;

namespace Authorization.Repository.IRepository
{
    public interface IUserRepository
    {
        Task Save();

        Task Add(User user);

        User Get(Expression<Func<User, bool>> filter, string? includeProperties = null);

        public Task<HashSet<Authorization.Enum.Permission>> GetUserPermission(Guid userId);
        public Task AddTest(User user);
        public Task<UserDTOTest> GetUser(string email);
    }
}
