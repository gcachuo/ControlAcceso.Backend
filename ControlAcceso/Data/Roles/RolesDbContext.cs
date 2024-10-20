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
}