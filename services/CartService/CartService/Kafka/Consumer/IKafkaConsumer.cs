namespace CartService.Kafka.Consumer
{
    public interface IKafkaConsumer
    {
        Task ConsumeAsync(string topic, CancellationToken stoppingToken);
    }
}
