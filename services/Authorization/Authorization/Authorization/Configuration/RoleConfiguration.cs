using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Authorization.Models;

namespace Authorization.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Models.Role>
    {
        public void Configure(EntityTypeBuilder<Models.Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<RolePermission>(
                l => l.HasOne<Models.Permission>().WithMany().HasForeignKey(e => e.PermissionId),
                r => r.HasOne<Models.Role>().WithMany().HasForeignKey(e => e.RoleId)
                );

            // код, який призначений для зберігання ролей у бд при запуску програми

            var roles = Enum.Role.GetValues<Enum.Role>().Select(r => new Models.Role
            {
                Id = (int)r,
                Name = r.ToString(),
            });

            builder.HasData(roles);
        }
    }
}