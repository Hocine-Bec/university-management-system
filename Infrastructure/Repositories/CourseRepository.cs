using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CourseRepository(AppDbContext context) : GenericRepository<Course>(context), ICourseRepository
{
    // Additional methods specific to Program can be added here
    public async Task<Course?> GetByCodeAsync(string code)
    {
        return await _context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Code == code);
    }

    public Task<bool> DoesCodeExistAsync(string code)
    {
        return _context.Courses.AnyAsync(c => c.Code == code);
    }
}