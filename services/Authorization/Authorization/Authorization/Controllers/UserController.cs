using Authorization.services;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public IActionResult Login(string email, string password)
        {
            var JwtToken = _userService.Login(email, password);
            return Ok(JwtToken);
        }
        [HttpPost("Register")]
        public IActionResult Register(string userName, string email, string password)
        {
            var JwtToken = _userService.Register(userName, email, password);
            return Ok(JwtToken);
        }
    }
}
