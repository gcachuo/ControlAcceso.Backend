using ControlAcceso.Data.Model;

namespace ControlAcceso.Data.RefreshTokens
{
    public interface IRefreshTokensDbContext
    {
        public void InsertToken(string refreshToken, int userId, string ipAddress, string userAgent);
    }
}