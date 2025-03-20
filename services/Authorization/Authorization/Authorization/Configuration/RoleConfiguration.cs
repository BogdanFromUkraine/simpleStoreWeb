using Authorization.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Models;

namespace Authorization.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<RolePermission>(
                l => l.HasOne<Permission>().WithMany().HasForeignKey(e => e.PermissionId),
                r => r.HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId)
                );

            // код, який призначений для зберігання ролей у бд при запуску програми

            var roles = Enum.Role.GetValues<Enum.Role>().Select(r => new Role
            {
                Id = (int)r,
                Name = r.ToString(),
            });

            builder.HasData(roles);
        }
    }
}