using CartService.Application.Interfaces;
using CartService.Kafka.Consumer;
using CartService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("check-auth")]
        [Authorize] // <--- Головний тригер для перевірки JWT
        public IActionResult CheckAuth()
        {
            // Якщо код дійшов сюди, значить токен ВАЛІДНИЙ!

            // Спробуємо витягнути дані з токена (наприклад, ID користувача)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst("sub")?.Value;

            return Ok(new
            {
                Message = "✅ Аутентифікація працює ідеально!",
                UserId = userId,
                // Виводимо всі розшифровані дані (Claims), щоб переконатися, що все прочиталось
                Claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
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