using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.Addresses
{
    public interface IAddressesDbContext
    {
        public List<AddressModel> SelectAddress();
        
        public void InsertAddress(AddressModel address);
    }

}
