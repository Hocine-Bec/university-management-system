namespace Applications.DTOs.Auth;

public record struct LoginRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}