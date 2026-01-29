using Authorization.Kafka.Producer;
using Authorization.services;
using Authorization.Services;
using CartService.DataAccess;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.IRepository;


namespace Authorization.Infrastructure
{
    public static class DependencyInjection
    {
        // Головний метод, який ти викличеш у Program.cs: builder.Services.AddInfrastructure(builder.Configuration);
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            services.AddAuthServices(configuration); // Специфічні сервіси для Authorization
            services.AddKafka(configuration);

            return services;
        }

        // 1. База даних та Репозиторії
        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Реєстрація DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    // Якщо міграції лежать в іншому проєкті, вкажи це, інакше можна прибрати цей рядок
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Реєстрація Репозиторіїв
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        // 2. Сервіси Аутентифікації та Логіки (Auth Specific)
        private static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Налаштування опцій (якщо в тебе є клас AuthorizationOptions)
            // services.Configure<AuthorizationOptions>(configuration.GetSection(nameof(AuthorizationOptions)));

            // Application Services (Бізнес-логіка)
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPermissionService, PermissionService>();

            // Infrastructure Services (Хешування, Токени)
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            // Authorization Handlers (Це важливо для PermissionRequirement)
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();


            return services;
        }

        // 3. Kafka (Messaging)
        private static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            var kafkaConfig = new ProducerConfig();
            configuration.GetSection("Kafka").Bind(kafkaConfig);

            services.AddSingleton(kafkaConfig);

            // Реєстрація Producer
            services.AddScoped<IKafkaProducer, UserEventProducer>();

            return services;
        }
    }
}