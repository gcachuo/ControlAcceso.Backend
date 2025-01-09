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
            var rows = DbService.ExecuteReader("""
                SELECT packages.*,
                    CASE
                        WHEN packages.status = 0 THEN 'recibido'
                        ELSE 'confirmado'
                    END as status_name,
                    CONCAT(addresses.street, ' #', addresses."number") as address
                FROM packages
                INNER JOIN addresses ON addresses.id = packages.address_id;
                """, new Dictionary<string, dynamic>());

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
                    Status = Convert.ToInt32(row["status"]),
                    StatusName = row["status_name"]?.ToString(),
                    Address = row["address"]?.ToString(),
                });
            }

            return packages;
        }
    }
}