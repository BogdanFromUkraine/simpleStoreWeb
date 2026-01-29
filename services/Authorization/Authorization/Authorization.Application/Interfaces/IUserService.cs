namespace Authorization.services
{
    public interface IUserService
    {
        Task Register(string userName, string email, string password);

        Task<string> Login(string email, string password);
    }
}