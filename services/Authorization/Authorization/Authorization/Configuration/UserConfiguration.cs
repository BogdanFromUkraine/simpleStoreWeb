using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Authorization.Models;
using ProductService.Models;

namespace Authorization.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        //створюється конфігурація для User, тобто створюється relation many to many
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRoles>(
                l => l.HasOne<Role>().WithMany().HasForeignKey(r => r.RoleId),
                r => r.HasOne<User>().WithMany().HasForeignKey(u => u.UserId));
        }
    }
}