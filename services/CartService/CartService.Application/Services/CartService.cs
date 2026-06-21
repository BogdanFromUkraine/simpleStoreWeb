using CartService.Application.Interfaces;
using CartService.Kafka.Consumer;
using CartService.Models;
using CartService.Repository.IRepository;

namespace CartService.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly IMessageStorageService _messageStorageService;
        private readonly IProductStockService _stockService;
        public CartService(ICartRepository repository, IMessageStorageService messageStorageService, IProductStockService stockService)
        {
            _repository = repository;
            _messageStorageService = messageStorageService;
            _stockService = stockService;
        }
        public async Task<Cart?> AddItemToCartAsync(string userId, int productId)
        {
            Guid guid = Guid.Parse(userId);
                
            int quantity = 200;
            string testproductId = productId.ToString();

            bool isAvailable = await _stockService.IsInStockAsync(testproductId, quantity);

            if (!isAvailable)
            {
                Console.WriteLine("Немає потрібної кількості товару на складі!");
            }
            else 
            {
                Console.WriteLine("Є у наявності");
            }

            var productsFromKafka = await _messageStorageService.GetAllMessages();
            //логіка знаходження product, тому що карт репосіторі, повинен працювати тільки з cart і не мати доступу до інших
            var product = productsFromKafka.SelectMany(p => p) // Розгортаємо всі списки у один
                              .FirstOrDefault(p => p.Id == productId); // Знаходимо першого користувача з заданим Id

            await _repository.Add(guid, product);

            return null;
        }

        public async Task<Cart?> GetUserCartAsync(string userId)
        {
            Guid guid = Guid.Parse(userId);

            var cart = await _repository.GetById(guid);

            if (cart == null)
            {
                throw new Exception("Cart не знайдено");
            }

            return cart;
        }

        public async Task RemoveItemFromCartAsync(string userId, int productId)
        {
            Guid guid = Guid.Parse(userId);

            var productsFromKafka = await _messageStorageService.GetAllMessages();
            var product = productsFromKafka.SelectMany(p => p) // Розгортаємо всі списки у один
                             .FirstOrDefault(p => p.Id == productId); // Знаходимо першого користувача з заданим Id
            await _repository.Remove(guid, product);

        }
    }
}
