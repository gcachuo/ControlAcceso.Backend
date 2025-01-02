using System.Collections.Generic;
using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Packages
{
    public interface IPackagesDbContext
    {
        List<PackageModel> SelectPackages();
    }
}
