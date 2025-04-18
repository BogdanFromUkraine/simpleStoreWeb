using ProductService.Models;

namespace CartService.Kafka.Consumer
{
    public interface IMessageStorageService
    {
        Task AddMessage(IEnumerable<Products> message);

        Task<IEnumerable<IEnumerable<Products>>> GetAllMessages();

        void ClearMessages();
    }
}