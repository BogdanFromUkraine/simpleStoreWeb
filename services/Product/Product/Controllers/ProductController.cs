using Authorization.Kafka.Producer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Interfaces;
using Product.Kafka.Consumer;
using Product.Models;
using ProductService.Models;
using System.Security.Claims;

namespace Product.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IEventProducer _eventProducer; // <-- Наш новий інтерфейс
        private readonly IMessageStorageService _messageStorageService;

        private readonly IProductService _productService;


        public ProductsController(
             IProductService productService,
             IEventProducer eventProducer,
             IMessageStorageService messageStorageService
             )
        {

            _eventProducer = eventProducer;
            _messageStorageService = messageStorageService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            var products = await _productService.GetProducts();

            //kafka producer
            await _eventProducer.SendMessageAsync("product-topic", "key", products);

            return Ok(products);
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

        [HttpGet("debug-auth")]
        [Authorize]
        public IActionResult DebugAuth()
        {
            // 1. Отримуємо стандартний заголовок
            var authHeader = Request.Headers["Authorization"].ToString();

            // 2. Отримуємо кастомний заголовок від OAuth2-Proxy
            var xAuthToken = Request.Headers["X-Forwarded-Access-Token"].ToString();

            // 3. Збираємо абсолютно всі заголовки для повної картини
            var allHeaders = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());

            return Ok(new
            {
                Message = "Перевірка токенів",
                FoundAuthorizationHeader = !string.IsNullOrEmpty(authHeader),
                AuthorizationValue = authHeader.Length > 20 ? authHeader.Substring(0, 20) + "..." : authHeader,
                FoundXForwardedToken = !string.IsNullOrEmpty(xAuthToken),
                AllHeaders = allHeaders
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProduct(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        //методи нижче будуть доступні тільки адміну

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductsDTO product)
        {
            await _productService.AddProduct(product);

            return Ok("все пройшло успішно");
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductsDTO product)
        {
            await _productService.UpdateProduct(id, product);

            return Ok("Все пройшло успішно");
        }

        [HttpDelete("{name}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(string name)
        {
            await _productService.RemoveProduct(name);
            return Ok("Все пройшло успішно");
        }
    }
}