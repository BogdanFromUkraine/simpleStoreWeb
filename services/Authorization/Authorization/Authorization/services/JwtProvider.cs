using Authorization.Models;
using Microsoft.IdentityModel.Tokens;
using Notes_project.Models.ModelsDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authorization.services
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _config;
        public JwtProvider(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(UserDTOTest user)
        {
            //створюю додаткову інформацію, щоб це скомпанувати це у JWT токен і потім витягнути додаткову інфу
            var claims = new List<Claim>()
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("userName", user.UserName.ToString()),
                new Claim("userEmail", user.Email.ToString()),
                new Claim("userEmail", user.Email.ToString())
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim("role", role.ToString()));
            }

            //алгоритм за яким буде кодуватися токен
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWTOptions:Key"])),
                SecurityAlgorithms.HmacSha256);
            //створення jwt токену
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(12));
            //конвертує jwt у рядок
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}
