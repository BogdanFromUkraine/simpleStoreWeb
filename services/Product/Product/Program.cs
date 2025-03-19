
using Authorization.Kafka.Producer;
using CartService.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product.Kafka.Consumer;
using Product.Repository;
using Product.Repository.IRepository;
using System.Text;


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
