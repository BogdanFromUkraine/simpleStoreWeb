namespace CartService.Kafka.Consumer
{
    public interface IEventConsumer
    {
        Task ConsumeAsync(string topic, CancellationToken stoppingToken);
    }
}