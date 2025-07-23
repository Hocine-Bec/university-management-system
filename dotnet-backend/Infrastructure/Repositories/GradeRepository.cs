using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GradeRepository(AppDbContext context) : GenericRepository<Grade>(context), IGradeRepository
{
    // Additional methods specific to Program can be added here
    public override async Task<Grade?> GetByIdAsync(int id)
    {
        return await _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Course)
            .Include(g => g.Semester)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IReadOnlyCollection<Grade>> GetByStudentIdAsync(int studentId)
    {
        return await _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Course)
            .Include(g => g.Semester)
            .AsNoTracking()
            .Where(g => g.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<Grade>> GetByCourseIdAsync(int courseId)
    {
        return await _context.Grades
            .Include(g => g.Student)
            .Include(g => g.Course)
            .Include(g => g.Semester)
            .AsNoTracking()
            .Where(g => g.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<bool> DoesExistAsync(int studentId, int courseId, int semesterId)
    {
        return await _context.Grades.AnyAsync(x => x.StudentId == studentId
                                                   && x.CourseId == courseId
                                                   && x.SemesterId == semesterId);
    }
}