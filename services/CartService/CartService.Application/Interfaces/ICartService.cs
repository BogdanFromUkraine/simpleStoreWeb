using CartService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Interfaces
{
    public interface ICartService
    {
        // Повертає кошик або null, якщо його не знайдено
        Task<Cart?> GetUserCartAsync(string userId);

        // Додає товар і повертає доданий продукт (або null/кидає помилку, якщо продукт не знайдено)
        Task<Cart?> AddItemToCartAsync(string userId, int productId);

        // Видаляє товар з кошика
        Task RemoveItemFromCartAsync(string userId, int productId);
    }
}
