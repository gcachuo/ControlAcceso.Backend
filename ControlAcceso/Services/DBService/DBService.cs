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

        public List<Dictionary<string, object>> Select(string selectQuery, Dictionary<string, dynamic> selectParameters)
        {
            try
            {
                _logger.LogInformation(selectQuery);

                _connection.Open(); // Abre la conexión

                // Crear el comando para la consulta SQL
                using var command = _connection.CreateCommand();
                command.CommandText = selectQuery;

                // Definir los parámetros con valores
                foreach (var parameter in selectParameters)
                {
                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = parameter.Key;
                    dbParameter.Value = parameter.Value;
                    command.Parameters.Add(dbParameter);
                }

                // Ejecutar la consulta y obtener los resultados usando ExecuteReader
                using var reader = command.ExecuteReader();

                // Crear una lista para almacenar los resultados
                var result = new List<Dictionary<string, object>>();

                // Leer los resultados fila por fila
                while (reader.Read())
                {
                    // Crear un diccionario para almacenar la fila actual
                    var row = new Dictionary<string, object>();

                    // Iterar por las columnas de la fila actual
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        // Agregar cada columna con su nombre y valor al diccionario
                        row[reader.GetName(i)] = reader.GetValue(i);
                    }

                    // Agregar la fila al resultado
                    result.Add(row);
                }

                return result;
            }
            finally
            {
                _connection.Close(); // Cierra la conexión
            }
        }

    }
}