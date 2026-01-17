using ProductService.Models;

namespace Authorization.Kafka.Producer
{
    public interface IEventProducer
    {
        Task SendMessageAsync(string topic, string key, IEnumerable<Products> message);

        void Dispose();
    }
}