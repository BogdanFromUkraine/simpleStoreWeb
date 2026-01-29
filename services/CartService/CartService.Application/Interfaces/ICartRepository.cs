using CartService.Models;
using ProductService.Models;

namespace CartService.Repository.IRepository
{
    public interface ICartRepository
    {
        Task<Cart> GetById(Guid userId);

        Task Add(Guid userId, Products product);

        Task Remove(Guid userId, Products product);
    }
}