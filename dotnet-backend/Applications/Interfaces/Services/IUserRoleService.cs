using Applications.DTOs;
using Applications.DTOs.UserRole;
using Applications.Shared;
using Domain.Enums;

namespace Applications.Interfaces.Services;

public interface IUserRoleService
{
    Task<Result<UserRoleDto>> AssignRoleAsync(UserRoleDto userRoleDto);
    Task<Result> RemoveRoleAsync(UserRoleDto userRoleDto);
    Task<Result<bool>> UserHasRoleAsync(UserRoleDto userRoleDto);
    Task<Result<bool>> UserHasRoleAsync(int userId, int roleType);
    Task<Result<UserRoleDto>> GetByUserAndRoleAsync(UserRoleDto userRoleDto);
    Task<Result<IReadOnlyCollection<UserRoleDto>>> GetByUserIdAsync(int userId);
    Task<Result<IReadOnlyCollection<UserRoleDto>>> GetByRoleIdAsync(int roleId);
    Task<Result<IReadOnlyCollection<RoleNameResponse>>> GetUserRoleNamesAsync(int userId);
}