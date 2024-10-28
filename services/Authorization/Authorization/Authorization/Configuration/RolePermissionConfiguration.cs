using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Authorization.Models;

namespace Authorization.Configuration
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        private readonly AuthorizationOptions _authorization;

        public RolePermissionConfiguration(AuthorizationOptions authorization)
        {
            _authorization = authorization;
        }

        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(r => new { r.RoleId, r.PermissionId });

            var rolePermissions = new RolePermission[]
         {
            new RolePermission { PermissionId = 1, RoleId = 1 },
            new RolePermission { PermissionId = 2, RoleId = 1 },
            new RolePermission { PermissionId = 3, RoleId = 1 },
            new RolePermission { PermissionId = 4, RoleId = 1 },
            new RolePermission { PermissionId = 1, RoleId = 2 },
             // Інші статичні записи
         };

            builder.HasData(rolePermissions);
        }

        private RolePermission[] ParseRolePermissions()
        {
            return _authorization.RolePermissions
                .SelectMany(rp => rp.Permissions
                .Select(p => new RolePermission
                {
                    PermissionId = (int)System.Enum.Parse<Enum.Permission>(p),
                    RoleId = (int)System.Enum.Parse<Enum.Role>(rp.Role)
                }))
                .ToArray();
        }
    }
}