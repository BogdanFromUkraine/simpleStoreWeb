
using Authorization.Kafka.Producer;
using CartService.DataAccess;
using Microsoft.EntityFrameworkCore;
using Product.Kafka.Consumer;
using Product.Repository;
using Product.Repository.IRepository;


namespace Product
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //DI
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            ////підключаю бд
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //реєстрація фонової служби
            builder.Services.AddHostedService<KafkaConsumerBackgroundService>();
            builder.Services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
            builder.Services.AddScoped<IKafkaProducer, ProductEventProducer>();
            builder.Services.AddSingleton<IMessageStorageService, MessageStorageService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
