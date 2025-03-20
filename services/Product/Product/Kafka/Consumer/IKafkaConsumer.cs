namespace Product.Kafka.Consumer
{
    public interface IKafkaConsumer
    {
        Task ConsumeAsync(string topic, CancellationToken stoppingToken);
    }
}