

using Product.Application.Interfaces;
using Product.Models;
using Product.Repository.IRepository;
using ProductService.Models;

namespace Product.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task AddProduct(ProductsDTO entity)
        {
            var product = new Products()
            {
                Name = entity.Name,
                Description = entity.Description,
                Price = decimal.Parse(entity.Price),
                Stock = int.Parse(entity.Stock),
                Cart = null,
                CartId = null,
            };

            await _repository.AddAsync(product);

        }

        public Task<Products> GetProductById(int id)
        {
            var product = _repository.GetByIdAsync(id);
            
            return product;
        }

        public Task<IEnumerable<Products>> GetProducts()
        {
            var products = _repository.GetAllAsync();
           
            return products;
        }

        public async Task RemoveProduct(string name)
        {
            var products = await GetProducts();

            await _repository
                .RemoveAsync(products.FirstOrDefault(p => p.Name == name));
        }

        public async Task UpdateProduct(int id, ProductsDTO productsDTO)
        {
            var product = await GetProductById(id);

           

            if (product != null)
            {
                product.Name = productsDTO.Name;
                product.Description = productsDTO.Description;
                product.Price = decimal.Parse(productsDTO.Price);
                product.Stock = int.Parse(productsDTO.Stock);

                await _repository.UpdateAsync(product);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
