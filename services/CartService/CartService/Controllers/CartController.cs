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
        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
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

        [HttpPost("{userId}")]
        public async Task<ActionResult<Cart>> AddToCart(Guid userId, [FromBody] int productId)
        {
            await _cartRepository.AddToCart(userId, productId);
            return Ok("Все пройшло успішно");
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult> RemoveFromCart(Guid userId, int productId)
        {
            await _cartRepository.RemoveFromCart(userId, productId);

            return Ok("Все пройшло успішно");
        }
    }
}
