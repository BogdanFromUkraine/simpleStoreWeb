using CartService.Models;
using CartService.Repository.IRepository;
using ProductService.Models;
using System.Collections.Concurrent; // Потрібно для потокобезпечного словника

namespace CartService.Repository
{
    public class InMemoryCartRepository : ICartRepository
    {
        // Це наша "база даних" у пам'яті.
        // Key (Guid) = UserId
        // Value (Cart) = Об'єкт кошика
        private readonly ConcurrentDictionary<Guid, Cart> _carts = new();

        public Task Add(Guid userId, Products product)
        {
            // GetOrAdd працює як магія:
            // "Знайди мені кошик цього юзера. Якщо його немає — створи новий порожній".
            var cart = _carts.GetOrAdd(userId, id => new Cart
            {
                UserId = id,
                Items = new List<Products>()
            });

            // Просто додаємо товар у список
            cart.Items.Add(product);

            // Повертаємо завершений Task, бо метод має бути асинхронним за інтерфейсом
            return Task.CompletedTask;
        }

        public Task<Cart> GetById(Guid userId)
        {
            // Пробуємо дістати значення. Якщо не знайде — поверне null (як FirstOrDefault у базі)
            _carts.TryGetValue(userId, out var cart);

            // Обгортаємо результат у Task, щоб задовольнити інтерфейс
            return Task.FromResult(cart);
        }

        public Task Remove(Guid userId, Products product)
        {
            // 1. Перевіряємо, чи є взагалі кошик у цього юзера
            if (_carts.TryGetValue(userId, out var cart))
            {
                // 2. Шукаємо товар у списку за ID
                // Важливо шукати саме об'єкт у списку, а не переданий об'єкт, бо посилання можуть бути різними
                var itemToRemove = cart.Items.FirstOrDefault(p => p.Id == product.Id);

                if (itemToRemove != null)
                {
                    cart.Items.Remove(itemToRemove);
                }
            }

            return Task.CompletedTask;
        }
    }
}