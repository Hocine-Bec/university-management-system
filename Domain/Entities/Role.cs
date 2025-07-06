using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

public class Role : IEntity
{
    public int Id { get; set; }
    public SystemRole Name { get; set; }
    public string? Description { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}