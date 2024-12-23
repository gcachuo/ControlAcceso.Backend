namespace ControlAcceso.Data.Model
{
    public class PackageModel
    {
        public int Id { get; set; }
        public string Service { get; set; }
        public DateTime ReceivedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public int AddressId { get; set; }
        public int Status { get; set; }
    }
}
