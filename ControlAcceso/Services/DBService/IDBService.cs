namespace ControlAcceso.Services.DBService
{
    public interface IDbService
    {
        public void Insert(string insertQuery, Dictionary<string, dynamic> insertParameters);

        public void Update(string updateQuery, Dictionary<string, dynamic> updateParameters);
    }
}