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

            // ������ Ocelot ������������
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:5173") // �������� ������ � React
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials() // �������� ���, ���� �������
                              .SetIsOriginAllowed(_ => true);
                    });
            });

            builder.Services.AddAuthentication("Bearer")
     .AddJwtBearer("Bearer", options =>
     {
         options.SaveToken = true;
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = false, // �� ��������, ��� ����� �����
             ValidateAudience = false, // �� ��������, ��� ���� �����
             ValidateLifetime = true, // ����� �� �� ���� ��������������
             ValidateIssuerSigningKey = true, // �������� ����� ������
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("c0mpL3xS3cur3K3y@98765432109876543210")) // ��������� ���� ��� �������� ������
         };
         options.Events = new JwtBearerEvents
         {
             OnAuthenticationFailed = context =>
             {
                 Console.WriteLine("? �������������� ���������: " + context.Exception.Message);
                 return Task.CompletedTask;
             },
             OnTokenValidated = context =>
             {
                 Console.WriteLine("? ����� ������� ��� " + context.Principal.Identity.Name);
                 return Task.CompletedTask;
             }
         };
     });

            builder.Services.AddAuthorization();

            // ������ Ocelot �� ������
            builder.Services.AddOcelot();

            var app = builder.Build();

            app.MapGet("/", () => Console.WriteLine("Hello World!"));

            //��� ������� body
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });

            app.UseHttpsRedirection(); // ��� ����� ������� ������ ����������� �� https

            // ������������� CORS
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();

            // ������������� Ocelot Middleware ��� ������� ������
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}