using ProductService.Models;
using Product.Repository.IRepository;
using CartService.DataAccess;
using Product.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Product.Repository
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Add(ProductsDTO entity)
        {
            var product = new Products()
            {
                Name = entity.Name,
                Description = entity.Description,
                Price = decimal.Parse( entity.Price),
                Stock = int.Parse(entity.Stock),
                Cart = null,
                CartId = null,

            };
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
        }

        public IEnumerable<Products> GetAll()
        {
            var products = _db.Products.ToList();


            return products;
        }

        public async Task Remove(string name) 
        {
            var product = _db.Products.FirstOrDefault(p => p.Name == name);
            _db.Remove(product);
            _db.SaveChangesAsync();
        }

        public async Task<Products> GetProduct(int id) 
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            _db.SaveChangesAsync();

            return product;
        }

        public async Task Update(int id, ProductsDTO productsDTO) 
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);

            //оновлення даних
            product.Name = productsDTO.Name;
            product.Description = productsDTO.Description;
            product.Price = decimal.Parse( productsDTO.Price);
            product.Stock = int.Parse(productsDTO.Stock);
            
            _db.Update(product);
            _db.SaveChangesAsync();
        }
    }
}
