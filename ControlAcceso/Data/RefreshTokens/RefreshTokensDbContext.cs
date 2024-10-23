using ControlAcceso.Services.DBService;

namespace ControlAcceso.Data.RefreshTokens
{
    public class RefreshTokensDbContext : IRefreshTokensDbContext
    {
        private IDbService DbService { get; set; }
        
        public RefreshTokensDbContext(IDbService dbService)
        {
            DbService = dbService;
        }
        public void InsertToken(string refreshToken, int userId, string ipAddress, string userAgent)
        {
            DbService.ExecuteNonQuery("""
                                         INSERT INTO refresh_tokens(token, user_id, created_at, expires_at, ip_address, user_agent)
                                         VALUES (@token, @user_id, @created_at, @expires_at, @ip_address, @user_agent)
                                      """,
                new()
                {
                    { "@token", refreshToken },
                    { "@user_id", userId },
                    { "@created_at", new DateTimeOffset(DateTime.UtcNow) },
                    { "@expires_at", new DateTimeOffset(DateTime.UtcNow).AddDays(7) },
                    { "@ip_address", ipAddress },
                    { "@user_agent", userAgent },
                });
        }
    }
}