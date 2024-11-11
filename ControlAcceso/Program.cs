using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ControlAcceso.Data.Addresses;
using ControlAcceso.Data.RefreshTokens;
using ControlAcceso.Data.Roles;
using ControlAcceso.Data.Users;
using ControlAcceso.Endpoints;
using ControlAcceso.Services.DBService;
using ControlAcceso.Tools.HttpContext;
using Microsoft.AspNetCore.Diagnostics;
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
                .AddScoped<IRolesDbContext, RolesDbContext>()
                .AddScoped<IAddressesDbContext, AddressesDbContext>();
            
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
                    appBuilder =>
                    {
                        appBuilder.AllowAnyOrigin()
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

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    // Obtener la excepción
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    var error = new ErrorResponse
                    {
                        Message = "Ha ocurrido un error en el servidor.",
                        Error = new()
                        {
                            Type = exceptionHandlerFeature?.Error.GetType().Name,
                            Message = exceptionHandlerFeature?.Error.Message,
                            Data = exceptionHandlerFeature?.Error.Data,
                        }
                    };

                    var jsonResponse = JsonSerializer.Serialize(error);
                    await context.Response.WriteAsync(jsonResponse);
                });
            });
            
            DotNetEnv.Env.Load();

            app.Run();
        }
    }

    internal class ErrorResponse : IResponse
    {
        public string? Message { get; set; }
        public ErrorEntity? Error { get; set; }

        internal class ErrorEntity
        {
            public string? Type { get; set; }
            public string? Message { get; set; }
            public IDictionary? Data { get; set; }
        }
    }
}