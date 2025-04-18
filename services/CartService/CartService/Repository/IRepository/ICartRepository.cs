using CartService.Models;
using ProductService.Models;

namespace CartService.Repository.IRepository
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserId(Guid userId);

        Task AddToCart(Guid userId, Products product);

        Task RemoveFromCart(Guid userId, Products product);
    }
}