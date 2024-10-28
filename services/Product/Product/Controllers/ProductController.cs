using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.DataAccess;
using Product.Models;
using Product.Repository.IRepository;

namespace Product.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(ApplicationDbContext context, IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            var products = _productRepository.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProduct(int id)
        {
            var product = _productRepository.Get(i => i.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        //методи нижче будуть доступні тільки адміну
        [HttpPost]
        public async Task<ActionResult<Products>> CreateProduct(Products product)
        {
            _productRepository.Add(product);
            await _productRepository.Save();
            return Ok("все пройшло успішно");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Products product)
        {
            var productFromDb = _productRepository.Get(i => i.Id == id);
            productFromDb = new Products() 
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
            };
            await _productRepository.Update(productFromDb);
            await _productRepository.Save();

            return Ok("Все пройшло успішно");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _productRepository.Get(i => i.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            await _productRepository.Remove(product);
            await _productRepository.Save();
            return Ok("Все пройшло успішно");
        }
    }

}
