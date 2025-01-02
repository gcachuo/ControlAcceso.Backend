using System.Collections.Generic;
using ControlAcceso.Data.Model;
using ControlAcceso.Services.DBService;
using Npgsql;

namespace ControlAcceso.Data.Packages
{
    public class PackagesDbContext : IPackagesDbContext
    {
        private IDbService DbService { get; set; }

        public PackagesDbContext(IDbService dbService)
        {
            DbService = dbService;
        }

        public List<PackageModel> SelectPackages()
        {
            var rows = DbService.ExecuteReader("SELECT * FROM packages WHERE status = 0", new Dictionary<string, dynamic>());
            var packages = new List<PackageModel>();

            foreach (var row in rows)
            {
                packages.Add(new PackageModel
                {
                    Id = Convert.ToInt32(row["id"]),
                    Service = row["service"]?.ToString(),
                    ReceivedAt = Convert.ToDateTime(row["received_at"]),
                    ConfirmedAt = row["confirmed_at"] != null ? Convert.ToDateTime(row["confirmed_at"]) : null,
                    AddressId = Convert.ToInt32(row["address_id"]),
                    Status = Convert.ToInt32(row["status"])
                });
            }

            return packages;
        }
    }
}
