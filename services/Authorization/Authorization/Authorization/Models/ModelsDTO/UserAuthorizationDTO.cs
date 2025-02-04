namespace Authorization.Models.ModelsDTO
{
    public class UserAuthorizationDTO
    {
        public string email { get; set; }
        public string password { get; set; }
        public string userName { get; set; } = string.Empty;
    }
}
