using System.Data;
using System.Diagnostics.CodeAnalysis;
using ControlAcceso.Data.RefreshTokens;
using ControlAcceso.Data.Roles;
using ControlAcceso.Data.Users;
using ControlAcceso.Services.DBService;
using ControlAcceso.Tools.HttpContext;
using Npgsql;

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
                .AddScoped<IHttpContext, Tools.HttpContext.HttpContext>()
                .AddScoped<IDbConnection, NpgsqlConnection>()
                .AddScoped<IRefreshTokensDbContext, RefreshTokensDbContext>()
                .AddScoped<IUsersDbContext, UsersDbContext>()
                .AddScoped<IRolesDbContext, RolesDbContext>();
            
            // Inyectar la configuración para obtener el connection string
            builder.Services.AddTransient<IDbConnection>(sp =>
            {
                var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
                return new NpgsqlConnection(connectionString); // Crear la conexión
            });

            builder.Services.AddTransient<IDbService, DbService>(); // Registrar DbService

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
            
            DotNetEnv.Env.Load();

            app.Run();
        }
    }
}