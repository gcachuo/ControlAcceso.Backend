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

        public List<GroupedPermission> GetGroupedPermissions(int roleId, int userId)
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

            return rows.Select(row => new GroupedPermission
            {
                Entity = row["entity"]?.ToString()!,
                Permissions = ((IEnumerable<object>)row["permissions"]!)
                                .Select(permission => permission.ToString()!)
                                .ToList()
            }).ToList();
        }
    }
}
