using Notes_project.Models.ModelsDTO;

namespace Authorization.services
{
    public interface IJwtProvider
    {
        string GenerateToken(UserDTOTest user);
    }
}