namespace ControlAcceso.Services.DBService
{
    public interface IDbService
    {
        public void Insert(string insertQuery, Dictionary<string, dynamic> insertParameters);
        public List<Dictionary<string, object>> Select(string selectQuery, Dictionary<string, dynamic> selectParameters);
    }
}