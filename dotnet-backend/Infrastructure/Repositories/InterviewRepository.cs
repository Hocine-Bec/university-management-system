using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InterviewRepository(AppDbContext context) : GenericRepository<Interview>(context), IInterviewRepository
{
    // Additional methods specific to Program can be added here
    public override Task<Interview?> GetByIdAsync(int id)
    {
        return _context.Interviews
            .AsNoTracking()
            .Include(x => x.Professor)
                .ThenInclude(x => x.Person)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
}