using Applications.DTOs;
using Applications.DTOs.UserRole;
using Domain.Entities;

namespace Applications.Helpers;

public static class ServicesExtension
{
    public static UserRole FromDto(this UserRoleDto role)
    {
        return new UserRole
        {
            Id = role.Id,
            UserId = role.UserId,
            RoleId = role.RoleId,
            IsActive = role.IsActive
        };
    }
    
    public static UserRoleDto ToDto(this UserRole role)
    {
        return new UserRoleDto
        {
            Id = role.Id,
            UserId = role.UserId,
            RoleId = role.RoleId,
            IsActive = role.IsActive
        };
    }
    
    public static RoleDto ToDto(this Role role)
    {
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description ?? string.Empty
        };
    }
}