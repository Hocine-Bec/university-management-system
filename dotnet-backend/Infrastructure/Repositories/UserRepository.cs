using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
{
    //User Specific Operations
    public async Task<bool> DeleteAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(s => s.Username == username);

        if (user == null)
            return false;

        _context.Users.Remove(user);
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task<bool> DoesExistAsync(int personId)
    {
        return await _context.Users.AnyAsync(x => x.PersonId == personId);
    }
    public async Task<IReadOnlyCollection<User>> GetByRoleAsync(int roleId)
    {
        return await _context.UserRoles
            .Where(ur => ur.RoleId == roleId && ur.IsActive)
            .Select(ur => ur.User)
            .ToListAsync();
    }
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .AsNoTracking()
            .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(n => n.Username == username);
    }
}