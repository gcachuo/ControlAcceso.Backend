namespace ControlAcceso.Services.DBService
{
    public interface IDbService
    {
    public void ExecuteNonQuery(string insertQuery, Dictionary<string, dynamic> insertParameters);    
            public List<Dictionary<string, object>> ExecuteReader(string selectQuery, Dictionary<string, dynamic> selectParameters);

    }

}