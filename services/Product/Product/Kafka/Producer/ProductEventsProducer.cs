namespace Authorization.Kafka.Producer
{
    using Confluent.Kafka;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using static Confluent.Kafka.ConfigPropertyNames;
    using Newtonsoft.Json;
    using ProductService.Models;

    public class ProductEventProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public ProductEventProducer()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
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
