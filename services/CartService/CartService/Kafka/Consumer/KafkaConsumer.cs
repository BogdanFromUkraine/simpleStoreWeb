using Confluent.Kafka;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using ProductService.Models;

namespace CartService.Kafka.Consumer
{
    public class KafkaConsumer : IKafkaConsumer
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
                    GroupId = "cart-group",
                    BootstrapServers = "localhost:9092",
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    MetadataMaxAgeMs = 60000,
                    EnableAutoCommit = true,
                    AutoCommitIntervalMs = 5000,
                    ReconnectBackoffMs = 1000,
                    ReconnectBackoffMaxMs = 10000
                };
                using var consumer = new ConsumerBuilder<Null, string>(config).Build();
                consumer.Subscribe(topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine("fsdfsd");
                    var consumeResult = consumer.Consume(stoppingToken);
                    var result = JsonConvert.DeserializeObject<IEnumerable<Products>>(consumeResult.Message.Value);

                    _messageStorageService.AddMessage(result);
                    Console.WriteLine(consumeResult.Message.Value);
                }
                consumer.Close();
            }
            catch (Exception)
            {
                //
                throw;
            }

        }

    }
}
