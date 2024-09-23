using System.Data;
using Npgsql;

namespace ControlAcceso.Services.DBService
{
    public class DbService : IDbService
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<DbService> _logger;

        public DbService(ILogger<DbService> logger, IDbConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        public void Insert(string insertQuery, Dictionary<string, dynamic> insertParameters)
        {
            try
            {
                _logger.LogInformation(insertQuery);

                _connection.Open(); // Abre la conexión

                // Crear el comando para la consulta SQL
                using var command = _connection.CreateCommand();
                command.CommandText = insertQuery;

                // Definir los parámetros con valores
                foreach (var parameter in insertParameters)
                {
                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = parameter.Key;
                    dbParameter.Value = parameter.Value;
                    command.Parameters.Add(dbParameter);
                }

                // Ejecutar la consulta de inserción
                var filasInsertadas = command.ExecuteNonQuery();
                _logger.LogInformation($"Inserted rows: {filasInsertadas}");
            }
            finally
            {
                _connection.Close(); // Cierra la conexión
            }
        }
    }
}