namespace ControlAcceso.Services.DBService
{
    public interface IDbService
    {
    public void ExecuteNonQuery(string insertQuery, Dictionary<string, dynamic> insertParameters);    
    
    }

}