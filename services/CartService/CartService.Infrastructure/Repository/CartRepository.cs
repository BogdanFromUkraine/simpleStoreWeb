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
            
        public async Task<Cart> GetById(Guid userId)
        {
            // Отримати кошик з бази даних за ідентифікатором користувача
            var cart = _db.Carts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.UserId == userId);
            return cart;
        }

        public async Task Add(Guid userId, Products product)
        {
            var cart = await GetById(userId);

            //шукаю product, який треба буде добавити до кошику
            //var productToCart = _db.Products.FirstOrDefault(p => p.Id == productId);

            // Додати продукт до кошика
            cart.Items.Add(product);
            await _db.SaveChangesAsync(); // Зберегти зміни в базі даних
        }

        public async Task Remove(Guid userId, Products product)
        {
            var cart = await GetById(userId);
            if (cart != null)
            {
                var productToRemove = cart.Items.FirstOrDefault(p => p.Id == product.Id);
                if (product != null)
                {
                    cart.Items.Remove(productToRemove);
                    await _db.SaveChangesAsync(); // Зберегти зміни в базі даних
                }
            }
        }
    }
}