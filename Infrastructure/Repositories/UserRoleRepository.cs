using Applications.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRoleRepository(AppDbContext context) : GenericRepository<UserRole>(context), IUserRoleRepository
{
    // Additional methods specific to UserRole can be added here
    public async Task<UserRole?> GetByUserAndRoleAsync(UserRole userRole)
    {
        return await _context.UserRoles
            .Include(ur => ur.User)
            .Include(ur => ur.Role)
            .FirstOrDefaultAsync(ur => ur.UserId == userRole.UserId && ur.RoleId == userRole.RoleId);
    }
    public async Task<IReadOnlyCollection<UserRole>> GetByUserIdAsync(int userId)
    {
        return await _context.UserRoles
            .Include(ur => ur.Role)
            .Where(ur => ur.UserId == userId && ur.IsActive)
            .ToListAsync();
    }
    public async Task<IReadOnlyCollection<UserRole>> GetByRoleIdAsync(int roleId)
    {
        return await _context.UserRoles
            .Include(ur => ur.User)
            .Where(ur => ur.RoleId == roleId && ur.IsActive)
            .ToListAsync();
    }
    public async Task<IReadOnlyCollection<SystemRole>> GetUserRoleNamesAsync(int userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId && ur.IsActive)
            .Select(ur => ur.Role.Name)
            .ToListAsync();
    }
    public async Task<UserRole> AssignRoleAsync(UserRole userRole)
    {
        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
        return userRole;
    }
    public async Task<bool> UserHasRoleAsync(UserRole userRole)
    {
        return await _context.UserRoles
            .AnyAsync(ur => ur.UserId == userRole.UserId && 
                            ur.RoleId == userRole.RoleId && 
                            ur.IsActive);
    }
    public async Task<bool> UserHasRoleAsync(int userId, SystemRole roleName)
    {
        return await _context.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.Role.Name == roleName && ur.IsActive);
    }
}