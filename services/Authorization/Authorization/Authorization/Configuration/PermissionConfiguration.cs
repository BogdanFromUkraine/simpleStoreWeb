using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Authorization.Models.Permission;

namespace Authorization.Configuration
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.Id);

            // код, який призначений для зберігання permission у бд при запуску програми
            var permission = Enum.Permission.GetValues<Enum.Permission>()
                .Select(p => new Permission
                {
                    Id = (int)p,
                    Name = p.ToString()
                });

            builder.HasData(permission);
        }
    }
}