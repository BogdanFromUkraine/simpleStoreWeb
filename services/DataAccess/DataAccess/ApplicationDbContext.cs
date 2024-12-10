
using Authorization.Models;
using CartService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductService.Models;
using System.Data;
using System.Security;


namespace CartService.DataAccess
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        public DbSet<User> User { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Products> Products { get; set; }


        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////цей код автоматично застосовує всі конфігурації
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}