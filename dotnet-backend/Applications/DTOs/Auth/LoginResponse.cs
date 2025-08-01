namespace Applications.DTOs.Auth;

//Application Layer
//DTO
public record struct LoginResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken  { get; set; }
    public DateTime AccessTokenExpires { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
}