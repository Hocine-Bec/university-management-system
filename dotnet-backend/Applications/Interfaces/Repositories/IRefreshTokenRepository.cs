using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(int userId);
    Task RevokeAllUserTokensAsync(int userId);
    Task RevokeTokenAsync(string token, string reason);
}