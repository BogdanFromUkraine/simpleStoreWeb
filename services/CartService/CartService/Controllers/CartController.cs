using CartService.Application.Interfaces;
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
        private readonly IEventConsumer _kafkaConsumer;

        private readonly ICartService _cartService;
        public CartController(ICartRepository cartRepository,
            IMessageStorageService messageStorageService,
            IEventConsumer kafkaConsumer,
            ICartService cartService)
        {
            _cartRepository = cartRepository;
            _messageStorageService = messageStorageService;
            _kafkaConsumer = kafkaConsumer;
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(string userId)
        {
            //Guid guid = Guid.Parse(userId);

            //var cart = await _cartRepository.GetCartByUserId(guid);
            //if (cart == null)
            //{
            //    return NotFound("Кошик не знайдено.");
            //}

            var cart = await _cartService.GetUserCartAsync(userId);

            return Ok(cart.Items);
        }

        [HttpPost("{userId}/items/{productId}")]
        public async Task<ActionResult<Cart>> AddToCart(string userId, int productId)
        {
            //var productsFromKafka = await _messageStorageService.GetAllMessages();
            ////логіка знаходження product, тому що карт репосіторі, повинен працювати тільки з cart і не мати доступу до інших
            //var product = productsFromKafka.SelectMany(p => p) // Розгортаємо всі списки у один
            //                  .FirstOrDefault(p => p.Id == productId); // Знаходимо першого користувача з заданим Id

           // await _cartService.AddItemToCartAsync(userId, product);
            //return Ok(product);
            return Ok();
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult> RemoveFromCart(string userId, int productId)
        {
            //Guid guid = Guid.Parse(userId);

            //var productsFromKafka = await _messageStorageService.GetAllMessages();
            //var product = productsFromKafka.SelectMany(p => p) // Розгортаємо всі списки у один
            //                 .FirstOrDefault(p => p.Id == productId); // Знаходимо першого користувача з заданим Id
            //await _cartService.RemoveItemFromCartAsync(guid, product);

            return Ok("Все пройшло успішно");
        }
    }
}