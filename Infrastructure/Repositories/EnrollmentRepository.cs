using Applications.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EnrollmentRepository(AppDbContext context) : GenericRepository<Enrollment>(context), IEnrollmentRepository
{
    // Additional methods specific to Program can be added here
    public override async Task<IReadOnlyCollection<Enrollment>> GetListAsync()
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Include(x => x.Program)
            .ToListAsync();
    }
    public override async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Include(x => x.Program)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<Enrollment?> GetByStudentIdAsync(int studentId)
    {
        return await _context.Enrollments
            .AsNoTracking()
            .Include(x => x.Program)
            .FirstOrDefaultAsync(x => x.StudentId == studentId);
    }

    public async Task<bool> CanEnrollInProgramAsync(int studentId, int programId)
    {
        return !await _context.Enrollments
            .AnyAsync(x => x.StudentId == studentId && 
                           x.ProgramId == programId &&
                           (x.Status == EnrollmentStatus.Active || 
                            x.Status == EnrollmentStatus.OnLeave ||
                            x.Status == EnrollmentStatus.Graduated));
    }
}