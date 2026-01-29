using CartService.Application.Interfaces;
using CartService.Models;
using CartService.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }
        public async Task<Cart?> AddItemToCartAsync(string userId, int productId)
        {
            //var productsFromKafka = await _messageStorageService.GetAllMessages();
            ////логіка знаходження product, тому що карт репосіторі, повинен працювати тільки з cart і не мати доступу до інших
            //var product = productsFromKafka.SelectMany(p => p) // Розгортаємо всі списки у один
            //                  .FirstOrDefault(p => p.Id == productId); // Знаходимо першого користувача з заданим Id


            //await _repository.Add(userId, product);

            //return product;

            throw new NotImplementedException();
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

            //var productsFromKafka = await _messageStorageService.GetAllMessages();
            //var product = productsFromKafka.SelectMany(p => p) // Розгортаємо всі списки у один
            //                 .FirstOrDefault(p => p.Id == productId); // Знаходимо першого користувача з заданим Id
            //await _repository.Remove(guid, product);

            throw new NotImplementedException();
        }
    }
}
