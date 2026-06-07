using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

            // Реєструємо сервіси аутентифікації та авторизації
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        // Динамічно зчитуємо хост Envoy (localhost:8888)
                        var proto = httpReq.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? httpReq.Scheme;
                        var host = httpReq.Headers["X-Forwarded-Host"].FirstOrDefault() ?? httpReq.Host.Value;

                        // Кнопка "Try it out" буде стріляти на http://localhost:8888/product
                        swaggerDoc.Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = $"{proto}://{host}/product" }
            };

                        // ХАК ДЛЯ ТВОГО ЕNVOY CONFIG:
                        // Оскільки Envoy для будь-якого запиту з префіксом "/product/" автоматично
                        // підставляє під капотом "/api/Product/", нам потрібно ОЧИСТИТИ згенеровані 
                        // шляхи Swagger від цього префікса, щоб уникнути дублювання.
                        var cleanedPaths = new OpenApiPaths();
                        foreach (var path in swaggerDoc.Paths)
                        {
                            // Відрізаємо "/api/Product" з початку кожного маршруту
                            var newKey = path.Key.Replace("/api/Product", "");

                            // Якщо після очищення шлях став порожнім (наприклад, був базовий маршрут), робимо його "/"
                            if (string.IsNullOrEmpty(newKey))
                            {
                                newKey = "/";
                            }

                            cleanedPaths.Add(newKey, path.Value);
                        }
                        swaggerDoc.Paths = cleanedPaths;
                    });
                });

                app.UseSwaggerUI(c =>
                {
                    // Відносний шлях без початкового слешу для правильного завантаження swagger.json
                    c.SwaggerEndpoint("v1/swagger.json", "Product API V1");

                    // .NET слухає чистий "/swagger", бо Envoy зрізає /product/swagger -> /swagger
                    c.RoutePrefix = "swagger";
                });
            }

            //app.UseHttpsRedirection();

            // Використовуємо CORS
            app.UseCors("AllowAll");
            app.Use(async (context, next) =>
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();
                var xAuthToken = context.Request.Headers["X-Forwarded-Access-Token"].ToString();

                Console.WriteLine("\n====================================================");
                Console.WriteLine($"[DEBUG LOG] Час: {DateTime.Now}");
                Console.WriteLine($"[DEBUG LOG] Шлях: {context.Request.Path}");

                if (!string.IsNullOrEmpty(authHeader))
                    Console.WriteLine($"[DEBUG LOG] Authorization: {authHeader}");
                else
                    Console.WriteLine("[DEBUG LOG] Authorization заголовок ВІДСУТНІЙ");

                if (!string.IsNullOrEmpty(xAuthToken))
                    Console.WriteLine($"[DEBUG LOG] X-Forwarded-Access-Token: {xAuthToken}");

                Console.WriteLine("====================================================\n");

                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}