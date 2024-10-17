using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Addresses
{
    public interface IAddressesDbContext
    {
        IEnumerable<AddressModel> SelectAddress(); 
    }

}
