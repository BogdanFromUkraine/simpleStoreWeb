using Authorization.Kafka.Producer;
using Authorization.Models.ModelsDTO;
using Authorization.services;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IKafkaProducer _kafkaProducer;
        public UserController(IUserService userService, IKafkaProducer kafkaProducer)
        {
            _userService = userService;
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserAuthorizationDTO data)
        {
            var JwtToken = await _userService.Login(data.email, data.password);

            //kafka producer
            _kafkaProducer.SendMessageAsync("user-topic", "key1", JwtToken);
            _kafkaProducer.Dispose();

            return Ok(JwtToken);
        }

        [HttpGet]
        public IActionResult Test()
        {
           
            return Ok("fdf");
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserAuthorizationDTO data)
        {
            var JwtToken = _userService.Register(data.userName, data.email, data.password);
            return Ok(JwtToken);
        }
    }
}
