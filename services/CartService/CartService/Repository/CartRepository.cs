using CartService.DataAccess;
using CartService.Models;
using CartService.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace CartService.Repository
{
    public class CartRepository : ICartRepository
    {
        private ApplicationDbContext _db;
        public CartRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Cart> GetCartByUserId(Guid userId)
        {
            // Отримати кошик з бази даних за ідентифікатором користувача
            var cart = _db.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId);
            return cart;
        }

        

        public async Task AddToCart(Guid userId, int productId)
        {
            var cart = await GetCartByUserId(userId);
           
            //шукаю product, який треба буде добавити до кошику
            var product = _db.Products.FirstOrDefault(p => p.Id == productId);


            // Додати продукт до кошика
            cart.Items.Add(product);
            await _db.SaveChangesAsync(); // Зберегти зміни в базі даних

        }

        public async Task RemoveFromCart(Guid userId, int productId)
        {
            var cart = await GetCartByUserId(userId);
            if (cart != null)
            {
                var product = cart.Items.FirstOrDefault(p => p.Id == productId);
                if (product != null)
                {
                    cart.Items.Remove(product);
                    _db.SaveChanges(); // Зберегти зміни в базі даних
                }
            }
        }
    }
}
