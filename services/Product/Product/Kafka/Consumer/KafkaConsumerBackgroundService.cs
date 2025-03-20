namespace Product.Kafka.Consumer
{
    public class KafkaConsumerBackgroundService : BackgroundService
    {
        private readonly IKafkaConsumer _consumerService;

        public KafkaConsumerBackgroundService(IKafkaConsumer consumerService)
        {
            _consumerService = consumerService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => _consumerService.ConsumeAsync("user-topic", stoppingToken), stoppingToken);
        }
    }
}