using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RegistrationRepository(AppDbContext context) : GenericRepository<Registration>(context), IRegistrationRepository
{
    // Additional methods specific to Program can be added here
    public override async Task<Registration?> GetByIdAsync(int id)
    {
        return await _context.Registrations
            .Include(r => r.Student)
            .Include(r => r.Section)
            .Include(r => r.Semester)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public Task<bool> DoesExistAsync(int studentId, int sectionId, int semesterId)
    {
        return _context.Registrations
            .AnyAsync(r => r.StudentId == studentId && 
                           r.SectionId == sectionId && 
                           r.SemesterId == semesterId);
    }
}