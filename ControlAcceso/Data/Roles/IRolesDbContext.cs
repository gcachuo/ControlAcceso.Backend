using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Roles;

public interface IRolesDbContext
{
    public void InsertRole(RoleModel role);

    public List<RoleModel> SelectRole();

    public RoleModel? SelectRoleById(int id);

    public void UpdateRoleName(int IdRole, RoleModel role);

}