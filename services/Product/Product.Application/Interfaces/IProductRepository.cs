using Product.Models;
using ProductService.Models;

namespace Product.Repository.IRepository
{
    public interface IProductRepository
    {
        // Приймаємо готову сутність, а не DTO
        Task AddAsync(Products product);

        // Повертаємо Task для асинхронності
        Task<IEnumerable<Products>> GetAllAsync();

        // Видаляємо конкретний об'єкт (його знайде сервіс і передасть сюди)
        Task RemoveAsync(Products product);

        // Пошук по ID
        Task<Products?> GetByIdAsync(int id);

        // Пошук по імені (потрібен для Service, щоб знайти перед видаленням)
        Task<Products?> GetByNameAsync(string name);

        // Оновлення цілої сутності
        Task UpdateAsync(Products product);
    }
}