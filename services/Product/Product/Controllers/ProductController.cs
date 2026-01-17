using Authorization.Kafka.Producer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Interfaces;
using Product.Kafka.Consumer;
using Product.Models;
using ProductService.Models;

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

        //[HttpGet("Test")]
        //public async Task<IActionResult> Test()
        //{
        //    var message = _messageStorageService.GetAllMessages();
        //    return Ok(message);
        //}

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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductsDTO product)
        {
            await _productService.AddProduct(product);

            return Ok("все пройшло успішно");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductsDTO product)
        {
            await _productService.UpdateProduct(id, product);

            return Ok("Все пройшло успішно");
        }

        [HttpDelete("{name}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(string name)
        {
            await _productService.RemoveProduct(name);
            return Ok("Все пройшло успішно");
        }
    }
}