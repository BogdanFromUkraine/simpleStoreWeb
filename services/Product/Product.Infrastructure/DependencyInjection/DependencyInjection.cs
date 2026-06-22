using Authorization.Kafka.Producer;
using CartService.DataAccess;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
            services.AddAuthAndCors(configuration);

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
           option.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddGrpc();

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

        public static IServiceCollection AddAuthAndCors(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:5173") // Дозволяє запити з React
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials()
                                .SetIsOriginAllowed(_ => true);
                    });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       // 1. Це адреса, яку .NET порівнюватиме з полем "iss" у токені (там у тебе http://localhost:9080/realms/demorealm)
       options.Authority = "http://localhost:9080/realms/demorealm";

       options.RequireHttpsMetadata = false;
       options.SaveToken = true;

       // 2. КРИТИЧНИЙ РЯДОК: Вказуємо реальну ВНУТРІШНЮ адресу Keycloak у Docker-мережі,
       // щоб .NET зміг фізично завантажити конфіг та публічні ключі (JWKS).
       // ЗАМІНИ 'keycloak' на ім'я сервісу Keycloak з твого docker-compose.yml, а 8080 — на його внутрішній порт.
       options.MetadataAddress = "http://keycloak:8080/realms/demorealm/.well-known/openid-configuration";

       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuerSigningKey = true,
           ValidateIssuer = true, // Поки тримаємо false для локального тесту
           ValidIssuer = "http://localhost:9080/realms/demorealm",
           ValidateAudience = true, // Поки тримаємо false
           ValidAudience = "democlient",
           ValidateLifetime = true,
           ClockSkew = TimeSpan.Zero,
           ValidTypes = new[] { "JWT", "ID" }
       };

       options.Events = new JwtBearerEvents
       {
           OnAuthenticationFailed = context =>
           {
               Console.WriteLine($"[JWT DEBUG] Помилка: {context.Exception.Message}");
               if (context.Exception.InnerException != null)
               {
                   Console.WriteLine($"[JWT DEBUG] Внутрішня: {context.Exception.InnerException.Message}");
               }
               return Task.CompletedTask;
           }
       };
   });

            services.AddAuthorization();

            return services;
        }
    }
}