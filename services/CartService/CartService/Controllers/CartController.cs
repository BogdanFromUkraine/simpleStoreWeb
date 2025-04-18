using CartService.Kafka.Consumer;
using CartService.Models;
using CartService.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMessageStorageService _messageStorageService;
        private readonly IKafkaConsumer _kafkaConsumer;

        public CartController(ICartRepository cartRepository,
            IMessageStorageService messageStorageService,
            IKafkaConsumer kafkaConsumer)
        {
            _cartRepository = cartRepository;
            _messageStorageService = messageStorageService;
            _kafkaConsumer = kafkaConsumer;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(string userId)
        {
            Guid guid = Guid.Parse(userId);

            var cart = await _cartRepository.GetCartByUserId(guid);
            if (cart == null)
            {
                return NotFound("Кошик не знайдено.");
            }
            return Ok(cart.Items);
        }

        [HttpPost("{userId}/items/{productId}")]
        public async Task<ActionResult<Cart>> AddToCart(Guid userId, int productId)
        {
            var productsFromKafka = await _messageStorageService.GetAllMessages();
            //логіка знаходження product, тому що карт репосіторі, повинен працювати тільки з cart і не мати доступу до інших
            var product = productsFromKafka.SelectMany(p => p) // Розгортаємо всі списки у один
                              .FirstOrDefault(p => p.Id == productId); // Знаходимо першого користувача з заданим Id

            await _cartRepository.AddToCart(userId, product);
            return Ok(product);
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult> RemoveFromCart(string userId, int productId)
        {
            Guid guid = Guid.Parse(userId);

            var productsFromKafka = await _messageStorageService.GetAllMessages();
            var product = productsFromKafka.SelectMany(p => p) // Розгортаємо всі списки у один
                             .FirstOrDefault(p => p.Id == productId); // Знаходимо першого користувача з заданим Id
            await _cartRepository.RemoveFromCart(guid, product);

            return Ok("Все пройшло успішно");
        }
    }
}