namespace Authorization.Kafka.Producer
{
    public interface IKafkaProducer
    {
        Task SendMessageAsync(string topic, string key, string message);

        void Dispose();
    }
}