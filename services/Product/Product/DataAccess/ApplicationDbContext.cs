using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Product.Models;


namespace Product.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Products> Products { get; set; }
      

        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
            ////цей код автоматично застосовує всі конфігурації
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            //modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
        }
    }
}