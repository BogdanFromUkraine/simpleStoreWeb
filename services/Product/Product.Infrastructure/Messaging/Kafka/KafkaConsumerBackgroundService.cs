using Microsoft.Extensions.Hosting;

namespace Product.Kafka.Consumer
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        private readonly IMessageConsumer _consumerService;

        public KafkaConsumerBackgroundService(IMessageConsumer consumerService)
        {
            _consumerService = consumerService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => _consumerService.ConsumeAsync("user-topic", stoppingToken), stoppingToken);
        }
    }
}