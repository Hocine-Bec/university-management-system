using Applications.Interfaces.Base;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Interfaces.Repositories;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    Task<UserRole?> GetByUserAndRoleAsync(UserRole userRole);
    Task<IReadOnlyCollection<UserRole>> GetByUserIdAsync(int userId);
    Task<IReadOnlyCollection<UserRole>> GetByRoleIdAsync(int roleId);
    Task<IReadOnlyCollection<SystemRole>> GetUserRoleNamesAsync(int userId);
    Task<UserRole> AssignRoleAsync(UserRole userRole);
    Task<bool> UserHasRoleAsync(UserRole userRole);
    Task<bool> UserHasRoleAsync(int userId, SystemRole roleName);
}