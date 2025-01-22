using ProductService.Models;

namespace Product.Kafka.Consumer
{
    public interface IMessageStorageService
    {
        void AddMessage(string message);
        IEnumerable<string> GetAllMessages();
        void ClearMessages();

    }
}
