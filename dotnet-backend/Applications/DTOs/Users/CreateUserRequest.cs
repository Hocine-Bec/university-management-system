using Domain.Enums;

namespace Applications.DTOs.Users;

public record struct CreateUserRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool IsActive { get; set; }
    public int PersonId { get; set; }
}