using Authorization.Repository.IRepository;
using CartService.DataAccess;
using CartService.Models;
using Microsoft.EntityFrameworkCore;
using Notes_project.Models.ModelsDTO;
using ProductService.Models;
using System.Linq.Expressions;

namespace Authorization.Repository
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _db;
        internal DbSet<User> dbSet;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<User>();
        }

        public async Task Add(User entity)
        {
            _db.Add(entity);
        }

        public User Get(Expression<Func<User, bool>> filter, string? includeProperties = null)
        {
            IQueryable<User> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public async Task Save()
        {
            _db.SaveChanges();
        }

        public async Task<HashSet<Authorization.Enum.Permission>> GetUserPermission(Guid userId)
        {
            try
            {
                var roles = _db.User
              .AsNoTracking()
              .Include(u => u.Roles)
              .ThenInclude(r => r.Permissions)
              .Where(u => u.Id == userId)
              .Select(u => u.Roles)
              .ToListAsync().GetAwaiter().GetResult();
                return roles
                    .SelectMany(r => r)
                    .SelectMany(r => r.Permissions)
                    .Select(p => (Authorization.Enum.Permission)p.Id)
                    //.Select(p =>
                    //{
                    //    if (int.TryParse(p.PermissionId.ToString(), out int permissionEnumValue) &&
                    //        Enum.IsDefined(typeof(PermissionEnum), permissionEnumValue))
                    //    {
                    //        var permissionEnum = (PermissionEnum)permissionEnumValue;
                    //        return ConvertToPermission(permissionEnum);
                    //    }
                    //    throw new ArgumentException($"Invalid PermissionId: {p.PermissionId}");
                    //})
                    .ToHashSet();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddTest(User user)
        {
            try
            {
                var roleEntity = _db.Roles.SingleOrDefaultAsync(r => r.Id == (int)Authorization.Enum.Role.User).GetAwaiter().GetResult();
                var userEntity = new User()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash,
                    Email = user.Email,
                    Roles = [roleEntity],
                    Cart = new Cart(),
                };
                _db.User.Add(userEntity);
                _db.SaveChangesAsync().GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDTOTest> GetUser(string email)
        {
            try
            {
                var user = _db.User
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Email == email);
                var userDto = new UserDTOTest
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash,
                    Roles = user.Roles.Select(r => r.Name).ToList() // Додаємо тільки назви ролей
                };
                return userDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}