using Npgsql;

namespace ControlAcceso.Services.DBService
{
    public class DbService : IDbService
    {
        private const string? ConnectionString = "Host=localhost;Username=postgres;Password=password;Database=postgres";
        private static readonly NpgsqlConnection Connection = new(ConnectionString);
        private readonly ILogger<DbService> _logger;

        public DbService(ILogger<DbService> logger)
        {
            _logger = logger;
        }

        public void Insert(string insertQuery, Dictionary<string, dynamic> insertParameters)
        {
            try
            {
                _logger.LogInformation(insertQuery);

                Connection.Open(); // Abre la conexión

                // Crear el comando para la consulta SQL
                using var command = new NpgsqlCommand(insertQuery, Connection);

                // Definir los parámetros con valores
                foreach (var parameter in insertParameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                // Ejecutar la consulta de inserción
                var filasInsertadas = command.ExecuteNonQuery();
                _logger.LogInformation($"Inserted rows: {filasInsertadas}");
            }
            finally
            {
                Connection.Close(); // Cierra la conexión
            }
        }
    }
}