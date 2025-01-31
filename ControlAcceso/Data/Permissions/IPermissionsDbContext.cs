using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Permissions;
public interface IPermissionsDbContext{
    public Dictionary<string,List<string>> GetRolePermissions(int roleId);
    public Dictionary<string,List<string>> GetGroupedPermissions(int roleId, int userId);
}