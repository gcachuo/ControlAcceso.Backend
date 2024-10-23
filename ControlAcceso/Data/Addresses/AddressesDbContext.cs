using System.Data;
using ControlAcceso.Data.Model;
using ControlAcceso.Services.DBService;
using Npgsql;

namespace ControlAcceso.Data.Addresses
{
    public class AddressesDbContext : IAddressesDbContext
    {
        private IDbService _dbService { get; set; }

        public AddressesDbContext(IDbService dbService)
        {
            _dbService = dbService;
        }

        public List<AddressModel> SelectAddress()
        {
            var addresses = new List<AddressModel>();

            
            var rows = _dbService.ExecuteReader("SELECT * FROM addresses", new Dictionary<string, dynamic>());

            foreach (var row in rows)
            {
                addresses.Add(new AddressModel
                {
                    Street = row["street"]?.ToString(),
                    Number = row["number"]?.ToString() 
                });
            }
           

            return addresses;
        }

        public void InsertAddress(AddressModel address)
        {
            try
            {
                _dbService.ExecuteNonQuery("""
                                              INSERT INTO Addresses(street, number)
                                              VALUES (@street, @number)
                                           """,
                    new()
                    {
                        { "@street", address.Street },
                        { "@number", address.Number },
                    }
                );
            }
            catch (PostgresException e)
            {
                if (e.Data["SqlState"]?.ToString() == "23505")
                {
                    throw new DataException("Rol duplicado.");
                }

                throw;
            }
        }
    }
}