using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DocsVerificationRepository(AppDbContext context)
    : GenericRepository<DocsVerification>(context), IDocsVerificationRepository
{
    // Additional methods specific to Program can be added here
    public async Task<DocsVerification?> GetByPersonIdAsync(int personId)
    {
        return await _context.DocsVerifications
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PersonId == personId);
    }
    
    public override async Task<IReadOnlyCollection<DocsVerification>> GetListAsync()
    {
        return await _context.DocsVerifications
            .AsNoTracking()
            .Include(src => src.Person)
            .ToListAsync();
    }

    public override async Task<DocsVerification?> GetByIdAsync(int id)
    {
        return await _context.DocsVerifications
            .AsNoTracking()
            .Include(src => src.Person)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}