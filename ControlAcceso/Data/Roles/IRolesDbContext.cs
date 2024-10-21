using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Roles;

public interface IRolesDbContext
{
    public void InsertRole(RoleModel role);

    public List<RoleModel> SelectRole();
}