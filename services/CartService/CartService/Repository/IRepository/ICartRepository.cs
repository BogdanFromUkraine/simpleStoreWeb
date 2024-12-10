using CartService.Models;
using ProductService.Models;

namespace CartService.Repository.IRepository
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserId(Guid userId);
       
        Task AddToCart(Guid userId, int productId);
        Task RemoveFromCart(Guid userId, int productId);
    }
}
