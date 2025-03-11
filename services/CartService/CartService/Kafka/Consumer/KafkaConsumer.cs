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
                    EnableAutoCommit = false,  // Вимикаємо автофіксацію офсетів
                    SessionTimeoutMs = 60000,  // Довше чекаємо перед розривом з'єднання
                    HeartbeatIntervalMs = 15000,  // Частіше перевіряємо з'єднання
                    SocketTimeoutMs = 120000,  // Таймаут для запитів
                    MaxPollIntervalMs = 300000,  // Більший інтервал між запитами, щоб Kafka не виключала клієнта
                    MetadataMaxAgeMs = 180000,
                };
                // Рідше оновлюємо мета-дані, щоб зменшити навантаження
                using var consumer = new ConsumerBuilder<Null, string>(config).Build();
                consumer.Subscribe(topic);

                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine("fsdfsd");
                    var consumeResult =  consumer.Consume(stoppingToken);
                    Console.WriteLine(consumeResult.Message.Value);

                    var result = JsonConvert.DeserializeObject<IEnumerable<Products>>(consumeResult.Message.Value);

                   

                    await _messageStorageService.AddMessage(result);
                    Console.WriteLine("close");

                    // Фіксуємо офсет вручну
                    consumer.Commit(consumeResult);
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
