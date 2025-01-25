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


            var app = builder.Build();

            app.MapGet("/", () => Console.WriteLine("Hello World!"));

            // ������������� Ocelot Middleware ��� ������� ������
            app.UseOcelot().GetAwaiter();

            app.Run();
        }
    }
}
