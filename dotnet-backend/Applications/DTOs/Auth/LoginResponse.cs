namespace Applications.DTOs.Auth;

//Application Layer
//DTO
public record struct LoginResponse
{
    public required string Token { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}