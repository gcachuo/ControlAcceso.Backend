using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Roles;

public interface IRolesDbContext
{
    public void InsertRole(RoleModel role);

    public List<RoleModel> SelectRole();
    
    public void UpdateRoleName(int id,RoleModel role);

    public bool RoleExists(int roleId);
    
}