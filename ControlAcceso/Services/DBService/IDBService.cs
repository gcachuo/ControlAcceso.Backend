namespace ControlAcceso.Services.DBService
{
    public interface IDbService
    {
        public void Insert(string insertQuery, Dictionary<string, dynamic> insertParameters);
    }
}