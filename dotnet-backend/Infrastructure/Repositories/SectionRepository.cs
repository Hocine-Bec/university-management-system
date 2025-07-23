using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SectionRepository(AppDbContext context) : GenericRepository<Section>(context), ISectionRepository
{
    // Additional methods specific to Program can be added here
    public async Task<bool> DeleteAsync(string sectionNumber)
    {
        var section = await _context.Sections.FirstOrDefaultAsync(x => x.SectionNumber == sectionNumber);

        if (section == null)
            return false;

        _context.Sections.Remove(section);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DoesExistAsync(string sectionNumber)
    {
        return await _context.Sections.AnyAsync(x => x.SectionNumber == sectionNumber);
    }

    public async Task<Section?> GetBySectionNumberAsync(string sectionNumber)
    {
        return await _context.Sections.FirstOrDefaultAsync(x => x.SectionNumber == sectionNumber);
    }
}