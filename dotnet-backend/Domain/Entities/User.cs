using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// System user account linking a person to system access with role-based permissions
/// </summary>
public class User : IEntity
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
     
    public int PersonId { get; set; }
    public required Person Person { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}