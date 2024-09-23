using System.Diagnostics.CodeAnalysis;
using ControlAcceso.Data.Users;
using ControlAcceso.Services.DBService;

namespace ControlAcceso
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {    
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOpenApi();
            
            builder.Services
                .AddScoped<IDbService, DbService>()
                .AddScoped<UsersDbContext>();

            builder.Services.AddControllers();
            
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.MapGet("/", () => "healthy").WithName("GetHealth");

            app.MapControllers();
            
            app.UseCors("AllowAllOrigins");

            app.Run();
        }
    }
}