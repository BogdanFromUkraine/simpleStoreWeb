using CartService.Kafka.Consumer;
using CartService.Models;
using CartService.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace CartService.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMessageStorageService _messageStorageService;
        public CartController(ICartRepository cartRepository, IMessageStorageService messageStorageService)
        {
            _cartRepository = cartRepository;
            _messageStorageService = messageStorageService;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(Guid userId)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                return NotFound("Кошик не знайдено.");
            }
            return Ok(cart.Items);
        }

        [HttpGet("Test")]
        public IActionResult Test() 
        {
            var message = _messageStorageService.GetAllMessages();
                    return Ok(message);
        }

        [HttpPost("{userId}/items/{productId}")]
        public async Task<ActionResult<Cart>> AddToCart(Guid userId, int productId)
        {
            var productsFromKafka =  _messageStorageService.GetAllMessages();
            //логіка знаходження product, тому що карт репосіторі, повинен працювати тільки з cart і не мати доступу до інших
            var product = productsFromKafka.SelectMany(p => p) // Розгортаємо всі списки у один
                              .FirstOrDefault(p => p.Id == productId); // Знаходимо першого користувача з заданим Id
            

            //await _cartRepository.AddToCart(userId, productId);
            return Ok(product);
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult> RemoveFromCart(Guid userId, int productId)
        {
            var productsFromKafka = _messageStorageService.GetAllMessages();
            var product = productsFromKafka.SelectMany(p => p) // Розгортаємо всі списки у один
                             .FirstOrDefault(p => p.Id == productId); // Знаходимо першого користувача з заданим Id
            await _cartRepository.RemoveFromCart(userId, product);

            return Ok("Все пройшло успішно");
        }
    }
}
