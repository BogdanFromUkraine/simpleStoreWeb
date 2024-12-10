namespace Authorization.Models
{
    public class UserRoles
    {
        //проміжна таблиця між User та Roles

        public Guid UserId { get; set; }
        public int RoleId { get; set; }
    }
}