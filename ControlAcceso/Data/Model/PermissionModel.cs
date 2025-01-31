namespace ControlAcceso.Data.Model
{
    public class PermissionModel
    {
        public string Entity { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new();
    }
}
