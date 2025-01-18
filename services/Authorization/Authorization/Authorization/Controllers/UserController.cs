using Authorization.Kafka.Producer;
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
        public IActionResult Login(string email, string password)
        {
            var JwtToken = _userService.Login(email, password);

           

            return Ok(JwtToken);
        }

        [HttpGet]
        public IActionResult Test()
        {
            //kafka producer
            _kafkaProducer.SendMessageAsync("my-topic", "key1", "Hello jfsd");
            _kafkaProducer.Dispose();
            return Ok("fdf");
        }

        [HttpPost("Register")]
        public IActionResult Register(string userName, string email, string password)
        {
            var JwtToken = _userService.Register(userName, email, password);
            return Ok(JwtToken);
        }
    }
}
