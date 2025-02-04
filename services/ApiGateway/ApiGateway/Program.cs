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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:5173") // �������� ������ � React
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // �������� ���, ���� �������
                    });
            });

            // ������ Ocelot �� ������
            builder.Services.AddOcelot();

           

            var app = builder.Build();

            app.MapGet("/", () => Console.WriteLine("Hello World!"));

            // ������������� CORS
            app.UseCors("AllowFrontend");

            //��� ������� body
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });

            // ������������� Ocelot Middleware ��� ������� ������
            app.UseOcelot().GetAwaiter();

            app.Run();
        }
    }
}
