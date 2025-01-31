using System.Data;
using ControlAcceso.Data.Model;
using ControlAcceso.Services.DBService;

namespace ControlAcceso.Data.Permissions
{
    public class PermissionsDbContext : IPermissionsDbContext
    {
        private IDbService DbService { get; set; }

        public PermissionsDbContext(IDbService dbService)
        {
            DbService = dbService;
        }

       public Dictionary<string, List<string>> GetRolePermissions(int roleId)
        {
            var query = @"
                SELECT entity, ARRAY_AGG(DISTINCT permission) AS permissions
                FROM role_permissions
                WHERE role_id = @RoleId
                GROUP BY entity";

            var parameters = new Dictionary<string, dynamic> { { "@RoleId", roleId } };
            var rows = DbService.ExecuteReader(query, parameters);

            var groupedPermissions = rows.Select(row => new PermissionModel
            {
                Entity = row["entity"]?.ToString()!,
                Permissions = (((string[])row["permissions"]) ?? Array.Empty<string>())
                                .ToList()
            }).ToList();

            return groupedPermissions.ToDictionary(gp => gp.Entity, gp => gp.Permissions);
        }

        public Dictionary<string,List<string>> GetGroupedPermissions(int roleId, int userId)
        {
            var query = @"
                SELECT entity, ARRAY_AGG(DISTINCT permission) AS permissions
                FROM (
                    SELECT entity, permission
                    FROM role_permissions
                    WHERE role_id = @RoleId
                    UNION ALL
                    SELECT entity, permission
                    FROM user_permissions
                    WHERE user_id = @UserId
                ) combined
                GROUP BY entity";

            var parameters = new Dictionary<string, dynamic>
            {
                { "@RoleId", roleId },
                { "@UserId", userId }
            };

            var rows = DbService.ExecuteReader(query, parameters);

            var groupedPermissions = rows.Select(row => new PermissionModel
            {
                Entity = row["entity"]?.ToString()!,
                Permissions = (((string[])row["permissions"]) ?? Array.Empty<string>())
                                .ToList()
            }).ToList();

            // Convertir al primer modelo (diccionario)
            var flatPermissions = groupedPermissions
                .ToDictionary(gp => gp.Entity, gp => gp.Permissions);

            return flatPermissions;    
        }
    }
}
