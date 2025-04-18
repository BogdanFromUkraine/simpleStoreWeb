using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

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
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials() // Дозволяє кукі, якщо потрібно
                              .SetIsOriginAllowed(_ => true);
                    });
            });

            builder.Services.AddAuthentication("Bearer")
     .AddJwtBearer("Bearer", options =>
     {
         options.SaveToken = true;
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = false, // Не перевіряє, хто видав токен
             ValidateAudience = false, // Не перевіряє, для кого токен
             ValidateLifetime = true, // Токен не має бути протермінованим
             ValidateIssuerSigningKey = true, // Перевіряє підпис токена
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c0mpL3xS3cur3K3y@98765432109876543210")) // Секретний ключ для валідації токенів
         };
         options.Events = new JwtBearerEvents
         {
             OnAuthenticationFailed = context =>
             {
                 Console.WriteLine("? Аутентифікація провалена: " + context.Exception.Message);
                 return Task.CompletedTask;
             },
             OnTokenValidated = context =>
             {
                 Console.WriteLine("? Токен валідний для " + context.Principal.Identity.Name);
                 return Task.CompletedTask;
             }
         };
     });

            builder.Services.AddAuthorization();

            // Додаємо Ocelot як службу
            builder.Services.AddOcelot();

            var app = builder.Build();

            app.MapGet("/", () => Console.WriteLine("Hello World!"));

            //для обробки body
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });

            app.UseHttpsRedirection(); // Цей метод примушує сервер редиректити на https

            // Використовуємо CORS
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();

            // Використовуємо Ocelot Middleware для обробки запитів
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}