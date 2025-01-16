using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Permissions;
public interface IPermissionsDbContext{
    public List<GroupedPermission> GetGroupedPermissions(int roleId, int userId);
}