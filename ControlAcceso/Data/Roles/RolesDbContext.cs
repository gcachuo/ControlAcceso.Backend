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
                Id = row["id"] as int?
            });
        }

        return roles;
    }

    public void UpdateRoleName(int id, RoleModel role)
    {
        try
        {
            var selectRole = "SELECT id FROM Roles WHERE id = @Id";
            var parameters = new Dictionary<string, dynamic>
            {
                { "@Id", id }
            };

            var result = DbService.ExecuteReader(selectRole, parameters);

            var updateQuery = @"
                UPDATE Roles
                SET name = @Name
                WHERE id = @Id";

            DbService.ExecuteNonQuery(updateQuery, new()
            {
                { "@Name", role.Name },
                { "@Id", id }
            });
        }
        catch (PostgresException e)
        {
            if (e.Data["SqlState"]?.ToString() == "23505")
            {
                throw new DataException("Ya existe un rol con ese nombre.");
            }
            throw;
        }
    }


}