using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Addresses
{
    public class Response:IResponse
    {
        public string? Message { get; set; }

        public IEnumerable<AddressModel>? Address { get; set; }
    }
}