using System.Data;
using System.Collections.Generic;
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

        public IEnumerable<AddressModel> SelectAddress()
        {
            var addresses = new List<AddressModel>();

            try
            {
                var rows = DbService.ExecuteReader("SELECT * FROM addresses", new Dictionary<string, dynamic>());

                foreach (var row in rows)
                {
                    addresses.Add(new AddressModel
                    {
                        Street = row["street"]?.ToString(),
                        Number = row["number"]?.ToString() 
                    });
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error al obtener direcciones: {e.Message}");
            }

            return addresses;
        }
    }
}
