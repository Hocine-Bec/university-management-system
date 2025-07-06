using Domain.Interfaces;

namespace Domain.Entities;

public class UserRole : IEntity
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public required User User { get; set; }
    
    public int RoleId { get; set; }
    public required Role Role { get; set; }
}