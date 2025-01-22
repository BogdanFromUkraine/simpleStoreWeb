using ProductService.Models;

namespace CartService.Kafka.Consumer
{
    public interface IMessageStorageService
    {
        void AddMessage(IEnumerable<Products> message);
        IEnumerable<IEnumerable<Products>> GetAllMessages();
        void ClearMessages();

    }
}
