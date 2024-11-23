using System.Data;
using ControlAcceso.Data.Model;
using ControlAcceso.Services.DBService;
using Npgsql;

namespace ControlAcceso.Data.Roles;

public class RolesDbContext:IRolesDbContext
{
    private IDbService DbService { get; set; }

    public RolesDbContext(IDbService dbService)
    {
        DbService = dbService;
    }

    public void InsertRole(RoleModel role)
    {
        try
        {
            DbService.ExecuteNonQuery("""
                                INSERT INTO Roles(name)
                                VALUES (@name)
                             """,
                new()
                {
                    { "@name", role.Name },
                }
            );
        }
        catch (PostgresException e)
        {
            if (e.Data["SqlState"]?.ToString() == "23505")
            {
                throw new DataException("Rol duplicado.");
            }

            throw;
        }
    }

    public List<RoleModel> SelectRole()
    {
        var rows = DbService.ExecuteReader("SELECT * FROM Roles", new Dictionary<string, dynamic>());
        var roles = new List<RoleModel>();

        foreach (var row in rows)
        {
            roles.Add(new RoleModel
            {
                Name = row["name"]?.ToString(),
                Id = row["id"]?.ToString()
            });
        }

        return roles;
    }

    public RoleModel? SelectRoleById(int id)
    {
        try
        {
            // Consulta SQL para obtener el rol por ID
            var query = "SELECT * FROM Roles WHERE id = @Id";

            // Ejecutar la consulta y obtener el resultado
            var rows = DbService.ExecuteReader(query, new()
            {
                { "@Id", id }
            });

            // Si no se encontraron resultados, devolver null
            if (!rows.Any())
            {
                return null;
            }

            // Tomar la primera fila y convertirla a RoleModel
            var row = rows.First();
            return new RoleModel
            {
                Id = row["id"]?.ToString(),
                Name = row["name"]?.ToString()
            };
        }
        catch (Exception ex)
        {
            throw new DataException("Error al obtener el rol por ID.", ex);
        }
    }

    public void UpdateRoleName(int IdRole, RoleModel role)
    {
        try
        {
            // Verificar si el rol existe
            var existingRole = SelectRoleById(IdRole);
            if (existingRole == null)
            {
                throw new DataException($"El rol con ID {IdRole} no existe.");
            }

            // Consulta SQL para actualizar el nombre del rol
            var updateQuery = @"
                UPDATE Roles
                SET name = @Name
                WHERE id = @Id";

            DbService.ExecuteNonQuery(updateQuery, new()
            {
                { "@Id", IdRole },
                { "@Name", role.Name }
            });
        }
        catch (PostgresException e)
        {
            if (e.Data["SqlState"]?.ToString() == "23505") // Código de error para entradas duplicadas
            {
                throw new DataException("Ya existe un rol con ese nombre.");
            }
            throw;
        }
    }


}