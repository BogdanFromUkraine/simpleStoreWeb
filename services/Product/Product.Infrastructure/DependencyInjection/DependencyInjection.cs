using Authorization.Kafka.Producer;
using CartService.DataAccess;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Interfaces;
using Product.Kafka.Consumer;
using Product.Repository;
using Product.Repository.IRepository;

namespace Project.Infrastructure
{
    public static class DependencyInjection
    {
        // Це ГОЛОВНИЙ метод, який викликає Program.cs
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Ми розбиваємо великий метод на менші шматочки для порядку
            services.AddPersistence(configuration);
            services.AddKafka(configuration);

            return services;
        }

        // 1. Окремий Extension метод для БАЗИ ДАНИХ (Persistence)
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Ось твій рядок, заради якого ми це робимо!
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, Product.Application.Services.ProductService>();

            // підключення до БД
            services.AddDbContext<ApplicationDbContext>(option =>
           option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        // 2. Окремий Extension метод для KAFKA (Messaging)
        public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Налаштування (Configuration)
            // Ми створюємо об'єкт конфігурації для Kafka і заповнюємо його даними з appsettings.json
            var kafkaConfig = new ClientConfig();
            configuration.GetSection("Kafka").Bind(kafkaConfig);

            // Специфічні налаштування для Consumer
            var consumerConfig = new ConsumerConfig(kafkaConfig);
            services.AddSingleton(consumerConfig);

            // Специфічні налаштування для Producer
            var producerConfig = new ProducerConfig(kafkaConfig);
            services.AddSingleton(producerConfig);

            // 2. Реєстрація Producer (Продюсера)
            // Зв'язуємо інтерфейс з Application і реалізацію з Infrastructure
            // У тебе клас може називатися ProductEventProducer або KafkaProducer
            services.AddSingleton<IEventProducer, ProductEventProducer>();

            services.AddSingleton<IMessageStorageService, MessageStorageService>();

            // 3. Реєстрація Consumer (Консюмера)
            // Реєструємо сам клас логіки читання
            services.AddSingleton<IMessageConsumer, KafkaConsumer>();

            // 4. Реєстрація Background Service (Фонова служба)
            // Це те, що фізично запустить метод ExecuteAsync і почне слухати топік
            services.AddHostedService<KafkaConsumerBackgroundService>();

            return services;
        }
    }
}