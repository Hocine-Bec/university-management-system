using Applications.DTOs.Auth;
using Domain.Entities;

namespace Applications.Interfaces.Auth;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    DateTimeOffset GetTokenExpiration();
}