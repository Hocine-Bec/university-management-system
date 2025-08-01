using Domain.Interfaces;

namespace Domain.Entities;

public class RefreshToken : IEntity
{
    public int Id { get; set; }
    public required string Token { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public string? RevokedReason { get; set; }
    public DateTime? RevokedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public bool IsActive => !IsRevoked && ExpiresAt > CreatedAt;
}