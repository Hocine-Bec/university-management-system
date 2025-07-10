using Domain.Enums;

namespace Applications.DTOs.Users;

public record struct UpdateUserRequest
{
    public string? Username { get; set; }
    public bool? IsActive { get; set; }
    public int? PersonId { get; set; }
}