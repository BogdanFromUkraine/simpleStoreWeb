using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using Product.Repository.IRepository;
using CartService.DataAccess;
using Product.Models;
using Product.Kafka.Consumer;
using Authorization.Kafka.Producer;

namespace Product.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMessageStorageService _messageStorageService;
        private readonly IKafkaProducer _kafkaProducer;


        public ProductsController(ApplicationDbContext context,
            IProductRepository productRepository,
            IMessageStorageService messageStorageService,
            IKafkaProducer kafkaProducer)
        {
            _productRepository = productRepository;
            _messageStorageService = messageStorageService;
            _kafkaProducer = kafkaProducer;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            var products = _productRepository.GetAll();

            //kafka producer
            _kafkaProducer.SendMessageAsync("product-topic", "key", products);
            _kafkaProducer.Dispose();

            return Ok(products);
        }

        [HttpGet("Test")]
        public IActionResult Test() 
        {
            var message = _messageStorageService.GetAllMessages();
            return Ok(message);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProduct(int id)
        {
            var product = _productRepository.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        //методи нижче будуть доступні тільки адміну
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductsDTO product)
        {
            await _productRepository.Add(product);

            return Ok("все пройшло успішно");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductsDTO product)
        {
            await _productRepository.Update(id, product);

            return Ok("Все пройшло успішно");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productRepository.Remove(id);
            return Ok("Все пройшло успішно");
        }
    }

}
