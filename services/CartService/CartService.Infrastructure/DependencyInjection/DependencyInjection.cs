using CartService.Application.Interfaces;
using CartService.DataAccess;
using CartService.Kafka.Consumer;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            Console.WriteLine(">>>>>>>>>> УВАГА! РЕЄСТРАЦІЯ ICartRepository <<<<<<<<<<");
            // Ось твій рядок, заради якого ми це робимо!
            services.AddScoped<CartService.Repository.IRepository.ICartRepository, CartService.Repository.CartRepository>();
            services.AddScoped<ICartService, CartService.Application.Services.CartService>();

            // підключення до БД
            services.AddDbContext<ApplicationDbContext>(option =>
           option.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        // 2. Окремий Extension метод для KAFKA (Messaging)
        public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Зчитуємо налаштування з appsettings.json
            var kafkaConfig = new ClientConfig();
            configuration.GetSection("Kafka").Bind(kafkaConfig);

            // 2. Реєструємо налаштування як Singleton, щоб їх можна було інжектити
            var consumerConfig = new ConsumerConfig(kafkaConfig);
            services.AddSingleton(consumerConfig);

            // =========================================================
            // 3. Реєстрація СЕРВІСУ ЗБЕРЕЖЕННЯ (Те, чого не вистачало)
            // =========================================================
            // Важливо: Singleton, тому що ти зберігаєш дані у List<> в пам'яті.
            // Якщо зробити Scoped, список буде очищатися при кожному запиті.
            services.AddSingleton<IMessageStorageService, MessageStorageService>();

           
            // 5. Реєстрація Consumer (Логіка читання)
            // Він залежить від MessageStorageService і ConsumerConfig
            services.AddSingleton<IEventConsumer, KafkaConsumer>();

            // 6. Реєстрація Background Service (Фонова служба)
            // Це "двигун", який запускає процес
            services.AddHostedService<KafkaConsumerBackgroundService>();

            return services;
        }
    }
}