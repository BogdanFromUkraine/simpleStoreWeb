using Product.Repository.IRepository;
using ProductService.Models;

namespace Product.Infrastructure.Mocks // Або твій простір імен
{
    public class InMemoryProductRepository : IProductRepository
    {
        // Це наша "База Даних". 
        // static важливий! Він гарантує, що список спільний для всіх запитів,
        // поки програма запущена.
        private static readonly List<Products> _storage = new();

        public Task AddAsync(Products product)
        {
            // Імітуємо Auto-Increment для ID, як в SQL
            if (_storage.Any())
            {
                product.Id = _storage.Max(p => p.Id) + 1;
            }
            else
            {
                product.Id = 1;
            }

            _storage.Add(product);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Products>> GetAllAsync()
        {
            // Повертаємо копію списку або сам список
            return Task.FromResult((IEnumerable<Products>)_storage);
        }

        public Task<Products?> GetByIdAsync(int id)
        {
            var product = _storage.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public Task<Products?> GetByNameAsync(string name)
        {
            // Ігноруємо регістр (великі/малі літери), щоб було як в БД
            var product = _storage.FirstOrDefault(p =>
                p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(product);
        }

        public Task RemoveAsync(Products product)
        {
            // Видаляємо об'єкт зі списку
            // Оскільки ми передаємо об'єкт, List спробує знайти саме його по посиланню.
            // Для надійності в In-Memory краще видаляти по ID:

            var itemToRemove = _storage.FirstOrDefault(p => p.Id == product.Id);
            if (itemToRemove != null)
            {
                _storage.Remove(itemToRemove);
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Products product)
        {
            // В List немає магії Entity Framework (Change Tracker).
            // Тому ми просто знаходимо старий запис, видаляємо і вставляємо оновлений.

            var existingIndex = _storage.FindIndex(p => p.Id == product.Id);

            if (existingIndex != -1)
            {
                _storage[existingIndex] = product;
            }

            return Task.CompletedTask;
        }
    }
}