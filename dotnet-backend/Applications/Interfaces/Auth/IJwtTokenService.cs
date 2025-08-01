using Domain.Entities;

namespace Applications.Interfaces.Auth;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    DateTime GetTokenExpiration();
    public RefreshToken GenerateRefreshToken(int userId);
    public bool ValidateRefreshToken(RefreshToken token);
}