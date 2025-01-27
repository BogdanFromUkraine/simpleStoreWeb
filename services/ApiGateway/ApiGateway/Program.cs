using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Додаємо Ocelot конфігурацію
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            // Додаємо Ocelot як службу
            builder.Services.AddOcelot();

            // Додаємо CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins("http://localhost:5173") // Дозволені джерела
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            app.MapGet("/", () => Console.WriteLine("Hello World!"));

            // Використовуємо CORS
            app.UseCors("AllowSpecificOrigins");

            // Використовуємо Ocelot Middleware для обробки запитів
            app.UseOcelot().GetAwaiter();

            app.Run();
        }
    }
}
