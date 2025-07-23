using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProgramRepository(AppDbContext context) : GenericRepository<Program>(context), IProgramRepository
{
    // Additional methods specific to Program can be added here
    public async Task<bool> DoesExistAsync(string code)
    {
        return await _context.Programs.AnyAsync(x => x.Code == code);
    }

    public async Task<bool> DeleteAsync(string code)
    {
        var program = await _context.Programs.FirstOrDefaultAsync(s => s.Code == code);

        if (program == null)
            return false;

        _context.Programs.Remove(program);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Program?> GetByCodeAsync(string code)
    {
        return await _context.Programs.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<bool> DoesExistsAsync(int id)
    {
        return await _context.Programs.AnyAsync(x => x.Id == id);
    }
}