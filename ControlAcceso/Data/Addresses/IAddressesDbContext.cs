
using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Addresses
{
    public interface IAddressesDbContext
    {
        AddressModel? SelectAddress();
    }
}
