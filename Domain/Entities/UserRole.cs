using Domain.Interfaces;

namespace Domain.Entities;

public class UserRole : IEntity
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}