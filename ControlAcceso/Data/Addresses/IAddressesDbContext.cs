using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Addresses
{
    public interface IAddressesDbContext
    {
        public void InsertAddress(AddressModel address);
    }
}