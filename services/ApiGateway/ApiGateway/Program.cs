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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:5173") // Дозволяє запити з React
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // Дозволяє кукі, якщо потрібно
                    });
            });

            // Додаємо Ocelot як службу
            builder.Services.AddOcelot();

           

            var app = builder.Build();

            app.MapGet("/", () => Console.WriteLine("Hello World!"));

            // Використовуємо CORS
            app.UseCors("AllowFrontend");

            //для обробки body
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });

            // Використовуємо Ocelot Middleware для обробки запитів
            app.UseOcelot().GetAwaiter();

            app.Run();
        }
    }
}
