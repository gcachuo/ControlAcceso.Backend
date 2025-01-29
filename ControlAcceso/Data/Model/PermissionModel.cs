namespace ControlAcceso.Data.Model;
public class GroupedPermission
{
    public string Entity { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
}
