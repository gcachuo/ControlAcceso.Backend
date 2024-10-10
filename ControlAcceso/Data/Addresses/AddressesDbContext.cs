using System.Data;
using ControlAcceso.Data.Model;
using ControlAcceso.Services.DBService;
using Npgsql;

namespace ControlAcceso.Data.Addresses
{
    public class AddressesDbContext : IAddressesDbContext
    {
        private IDbService DbService { get; set; }

        public AddressesDbContext(IDbService dbService)
        {
            DbService = dbService;
        }

        public AddressModel? SelectAddress()
        {
            var row = DbService.ExecuteReader("SELECT * FROM addresses LIMIT 1").SingleOrDefault(); 
            if (row == null)
                return null;

            return new AddressModel
            {
                Street = row["street"]?.ToString(),
                Number = row["number"]?.ToString()
            };
        }
    }
}
