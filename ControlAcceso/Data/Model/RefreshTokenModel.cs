namespace ControlAcceso.Data.Model
{
    public class RefreshTokenModel
    {
        public int? Id { get; set; }
        public string? Token { get; set; }
        public int? UserId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public bool? IsValid { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTimeOffset? LastUsedAt { get; set; }
    }
}