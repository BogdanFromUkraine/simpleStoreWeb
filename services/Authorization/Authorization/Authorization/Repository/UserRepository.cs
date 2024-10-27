using Authorization.DataAccess;
using Authorization.Models;
using Authorization.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
    }
}
