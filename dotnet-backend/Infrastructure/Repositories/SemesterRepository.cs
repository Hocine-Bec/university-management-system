using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SemesterRepository(AppDbContext context) : GenericRepository<Semester>(context), ISemesterRepository
{
    // Additional methods specific to Program can be added here
    public async Task<Semester?> GetByTermCodeAsync(string termCode)
    {
        return await _context.Semesters
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.TermCode == termCode); 
    }
}