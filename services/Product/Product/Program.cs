using Authorization.Kafka.Producer;
using CartService.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product.Application.Interfaces;
using Product.Application.Services;
using Product.Infrastructure.Mocks;
using Product.Repository;
using Product.Repository.IRepository;
using Project.Infrastructure;
using System.Text;

namespace Product
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddInfrastructure(builder.Configuration);


            //Перевірка через In-memory
            //bool useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

            //if (useInMemory)
            //{
            //    // === РЕЖИМ IN-MEMORY (Тестовий) ===
            //    Console.WriteLine("⚠️ УВАГА: Використовується In-Memory Product Repository!");

            //    // Реєструємо мок як Singleton.
            //    // Singleton означає "один екземпляр на все життя програми".
            //    // Для In-Memory це ідеально, хоча твій список і так static, але це правильна практика.
            //    builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
            //}
            //else
            //{
            //    // === РЕАЛЬНИЙ РЕЖИМ (SQL Database) ===
            //    Console.WriteLine("✅ Підключено до реальної бази даних.");

                
            //}
            //builder.Services.AddScoped<IProductService, Product.Application.Services.ProductService>();


            // Додаємо CORS, щоб не блокувало Ocelot
            builder.Services.AddCors(options =>
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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //код який перевіряє JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c0mpL3xS3cur3K3y@98765432109876543210")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Використовуємо CORS
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}