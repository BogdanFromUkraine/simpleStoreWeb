using ProductService.Models;

namespace Authorization.Kafka.Producer
{
    public interface IKafkaProducer
    {
        Task SendMessageAsync(string topic, string key, IEnumerable<Products> message);

        void Dispose();
    }
}