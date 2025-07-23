using Applications.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository(AppDbContext context) : GenericRepository<Role>(context), IRoleRepository
{
    // Additional methods specific to Role can be added here
    public async Task<Role?> GetByNameAsync(SystemRole name)
    {
        return await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == name);
    }
}