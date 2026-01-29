using Microsoft.Extensions.Hosting;

namespace CartService.Kafka.Consumer
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        private readonly IEventConsumer _consumerService;

        public KafkaConsumerBackgroundService(IEventConsumer consumerService)
        {
            _consumerService = consumerService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => _consumerService.ConsumeAsync("product-topic", stoppingToken), stoppingToken);
        }
    }
}