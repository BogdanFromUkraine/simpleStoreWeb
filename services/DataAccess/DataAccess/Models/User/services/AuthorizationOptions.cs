namespace Authorization
{
    //цей клас створений, щоб получити інформацію з appsettings, а саме AuthorizationOptions
    //щоб получити ці дані, я буду використовувати IOptions
    public class AuthorizationOptions
    {
        public RolePermissions[] RolePermissions { get; set; } = [];
    }

    public class RolePermissions
    {
        public string Role { get; set; } = string.Empty;

        public string[] Permissions { get; set; } = [];
    }
}