using Confluent.Kafka;

namespace Product.Kafka
{
    public class KafkaConsumer : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => 
            {
                _ = ConsumeAsync("my-topic", stoppingToken);
            }, stoppingToken);
        }

        public async Task ConsumeAsync(string topic, CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "product-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);

            while (!stoppingToken.IsCancellationRequested) 
            {
                var consumeResult = consumer.Consume(stoppingToken);
                Console.WriteLine(consumeResult.Message.Value);
            }
            consumer.Close();
        }
       
    }
}
