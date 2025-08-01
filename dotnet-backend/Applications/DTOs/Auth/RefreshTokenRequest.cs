namespace Applications.DTOs.Auth;

public record struct RefreshTokenRequest
{
    public required string RefreshToken { get; set; }
}