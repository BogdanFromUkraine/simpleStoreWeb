namespace Authorization.Kafka.Producer
{
    using Confluent.Kafka;
    using Newtonsoft.Json;
    using ProductService.Models;
    using System.Threading.Tasks;

    public class ProductEventProducer : IEventProducer
    {
        private readonly IProducer<Null, string> _producer;

        public ProductEventProducer()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "kafka:29092",
                Acks = Acks.All,  // Очікуємо підтвердження від всіх брокерів
                MessageTimeoutMs = 60000,  // Довше чекаємо підтвердження від брокера
                RetryBackoffMs = 500,  // Час очікування між повторними спробами
                EnableIdempotence = true,  // Включаємо ідємпотентність (повторні відправки не дублюються)
                MaxInFlight = 5  // Контролюємо кількість одночасних запитів
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task SendMessageAsync(string topic, string key, IEnumerable<Products> message)
        {
            try
            {
                var deliveryResult = await _producer.ProduceAsync(topic, new Message<Null, string>
                {
                    Value = JsonConvert.SerializeObject(message)
                });
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Failed to deliver message: {ex.Error.Reason}");
            }
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}