using Product.Models;
using ProductService.Models;

namespace Product.Application.Interfaces
{
    public interface IProductService
    {
        Task AddProduct(ProductsDTO entity);
        Task<IEnumerable<Products>> GetProducts();
        Task RemoveProduct(string name);
        Task<Products> GetProductById(int id);
        Task UpdateProduct(int id, ProductsDTO productsDTO);

    }
}
