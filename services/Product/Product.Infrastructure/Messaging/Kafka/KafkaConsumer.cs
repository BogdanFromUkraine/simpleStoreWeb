using Confluent.Kafka;

namespace Product.Kafka.Consumer
{
    public class KafkaConsumer : IMessageConsumer
    {
        private readonly IMessageStorageService _messageStorageService;

        public KafkaConsumer(IMessageStorageService messageStorageService)
        {
            _messageStorageService = messageStorageService;
        }
        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            try
            {
                var config = new ConsumerConfig
                {
                    GroupId = "product-group",
                    BootstrapServers = "kafka:29092",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };
                using var consumer = new ConsumerBuilder<Null, string>(config).Build();
                consumer.Subscribe(topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    //var result = JsonConvert.DeserializeObject<User>(consumeResult.Message.Value);

                    _messageStorageService.AddMessage(consumeResult.Message.Value);
                    Console.WriteLine(consumeResult.Message.Value);
                }
                consumer.Close();
            }
            catch (Exception ex)
            {
                // Глобальна помилка підключення (наприклад, брокер лежить)
                Console.WriteLine($"Kafka connection failed: {ex.Message}. Retrying in 10s...");
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}