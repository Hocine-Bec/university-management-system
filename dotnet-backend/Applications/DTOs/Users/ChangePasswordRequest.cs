namespace Applications.DTOs.Users;

public record struct ChangePasswordRequest
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}