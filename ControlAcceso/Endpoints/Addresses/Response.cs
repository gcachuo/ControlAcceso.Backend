using ControlAcceso.Data.Model;

namespace ControlAcceso.Endpoints.Addresses
{
    public class Response:IResponse
    {
        public string? Message { get; set; }

        public IEnumerable<AddressModel>? Addresses { get; set; }
    }
}