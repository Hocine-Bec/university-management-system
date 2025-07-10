using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Applications.Interfaces.Auth;
using Domain.Entities;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        ValidateSettings();
    }
    
    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = GetUserClaims(user);
        
        var token = new JwtSecurityToken
        (
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);   
    }
    
    private void ValidateSettings()
    {
        if (string.IsNullOrWhiteSpace(_jwtSettings.Secret))
            throw new InvalidOperationException("JWT Secret is not configured");

        if (_jwtSettings.Secret.Length < 32)
            throw new InvalidOperationException("JWT Secret must be at least 32 characters long");

        if (string.IsNullOrWhiteSpace(_jwtSettings.Issuer))
            throw new InvalidOperationException("JWT Issuer is not configured");

        if (string.IsNullOrWhiteSpace(_jwtSettings.Audience))
            throw new InvalidOperationException("JWT Audience is not configured");

        if (_jwtSettings.LifeTime <= 0)
            throw new InvalidOperationException("JWT Lifetime must be greater than 0");
    }
    
    public DateTimeOffset GetTokenExpiration()
    {
        return DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.LifeTime);
    }
    
    private List<Claim> GetUserClaims(User user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.Username),
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(),
                ClaimValueTypes.Integer64)
        };
        
        foreach (var role in user.UserRoles)
        {
            claims.Add(new(ClaimTypes.Role, role.Role.Name.ToString()));
        }

        return claims;
    }
}