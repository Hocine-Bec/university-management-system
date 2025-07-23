using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FinancialHoldRepository(AppDbContext context) : GenericRepository<FinancialHold>(context), IFinancialHoldRepository
{
    // Additional methods specific to Program can be added here
    public async Task<IReadOnlyCollection<FinancialHold>> GetByStudentIdAsync(int studentId)
    {
        return await _context.FinancialHolds
            .AsNoTracking()
            .Where(x => x.StudentId == studentId)
            .ToListAsync();
    }
}