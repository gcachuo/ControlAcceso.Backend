using System.Diagnostics.CodeAnalysis;

namespace ControlAcceso
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {    
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOpenApi();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.MapGet("/", () => "healthy").WithName("GetHealth");

            app.MapControllers();

            app.Run();
        }
    }
}