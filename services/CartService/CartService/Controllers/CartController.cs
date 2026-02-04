using CartService.Application.Interfaces;
using CartService.Kafka.Consumer;
using CartService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(
            IEventConsumer kafkaConsumer,
            ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(string userId)
        {
            var cart = await _cartService.GetUserCartAsync(userId);

            return Ok(cart.Items);
        }

        [HttpPost("{userId}/items/{productId}")]
        public async Task<ActionResult<Cart>> AddToCart(string userId, int productId)
        {
            await _cartService.AddItemToCartAsync(userId, productId);

            return Ok();
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult> RemoveFromCart(string userId, int productId)
        {
            await _cartService.RemoveItemFromCartAsync(userId, productId);

            return Ok("Все пройшло успішно");
        }
    }
}