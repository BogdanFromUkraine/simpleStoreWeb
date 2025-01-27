using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ������ Ocelot ������������
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            // ������ Ocelot �� ������
            builder.Services.AddOcelot();

            // ������ CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins("http://localhost:5173") // �������� �������
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            app.MapGet("/", () => Console.WriteLine("Hello World!"));

            // ������������� CORS
            app.UseCors("AllowSpecificOrigins");

            // ������������� Ocelot Middleware ��� ������� ������
            app.UseOcelot().GetAwaiter();

            app.Run();
        }
    }
}
