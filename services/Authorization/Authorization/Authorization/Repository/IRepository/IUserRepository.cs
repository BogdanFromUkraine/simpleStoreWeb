using Authorization.Models;
using System.Linq.Expressions;

namespace Authorization.Repository.IRepository
{
    public interface IUserRepository
    {
        Task Save();

        Task Add(User user);

        User Get(Expression<Func<User, bool>> filter, string? includeProperties = null);
    }
}
