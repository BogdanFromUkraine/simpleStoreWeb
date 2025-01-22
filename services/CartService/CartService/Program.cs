
using CartService.DataAccess;
using Microsoft.EntityFrameworkCore;
using CartService.DataAccess;
using CartService.Repository.IRepository;
using CartService.Repository;
using CartService.Kafka.Consumer;

namespace CartService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //підключаю бд
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<ICartRepository, CartRepository>();

            //реєстрація фонової служби
            builder.Services.AddHostedService<KafkaConsumerBackgroundService>();
            builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
            builder.Services.AddSingleton<IMessageStorageService, MessageStorageService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
