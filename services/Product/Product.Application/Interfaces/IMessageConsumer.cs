namespace Product.Kafka.Consumer
{
    public interface IMessageConsumer
    {
        Task ConsumeAsync(string topic, CancellationToken stoppingToken);
    }
}