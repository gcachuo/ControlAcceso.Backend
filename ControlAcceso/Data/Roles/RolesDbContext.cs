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
            DbService.Insert("""
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
        }
    }
}