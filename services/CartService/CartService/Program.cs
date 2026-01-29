using CartService.DataAccess;
using CartService.Kafka.Consumer;
using CartService.Repository;
using CartService.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Project.Infrastructure;

namespace CartService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //підключаю бд
            //builder.Services.AddDbContext<ApplicationDbContext>(option =>
            //option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddScoped<ICartRepository, CartRepository>();

            ////реєстрація фонової служби
            //builder.Services.AddHostedService<KafkaConsumerBackgroundService>();
            //builder.Services.AddSingleton<IEventConsumer, KafkaConsumer>();
            //builder.Services.AddSingleton<IMessageStorageService, MessageStorageService>();

            // Додаємо CORS, щоб не блокувало Ocelot
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOcelot",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Використовуємо CORS
            app.UseCors("AllowOcelot");

            app.MapControllers();

            app.Run();
        }
    }
}