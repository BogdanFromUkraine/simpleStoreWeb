using Confluent.Kafka;

namespace Product.Kafka.Consumer
{
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly IMessageStorageService _messageStorageService;

        public KafkaConsumer(IMessageStorageService messageStorageService)
        {
            _messageStorageService = messageStorageService;
        }

        //protected override Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    return Task.Run(() =>
        //    {
        //        _ = ConsumeAsync("my-topic", stoppingToken);
        //    }, stoppingToken);
        //}

        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            try
            {
                var config = new ConsumerConfig
                {
                    GroupId = "product-group",
                    BootstrapServers = "localhost:9092",
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
            catch (Exception)
            {
                //
                throw;
            }
        }
    }
}