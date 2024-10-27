using Authorization.Models;

namespace Authorization.services
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
