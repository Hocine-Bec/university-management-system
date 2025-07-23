using Domain.Enums;

namespace Applications.DTOs;

public record struct RoleDto
{
    public int Id { get; set; }
    public SystemRole Name { get; set; }
    public string Description { get; set; }
}   