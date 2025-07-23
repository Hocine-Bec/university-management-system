using System.Runtime.InteropServices.ComTypes;
using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PrerequisiteRepository(AppDbContext context) : GenericRepository<Prerequisite>(context), IPrerequisiteRepository
{
    // Additional methods specific to Program can be added here
    public override Task<Prerequisite?> GetByIdAsync(int id)
    {
        return _context.Prerequisites
            .AsNoTracking()
            .Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public Task<Prerequisite?> GetByCourseIdAsync(int courseId)
    {
        return _context.Prerequisites
            .AsNoTracking()
            .Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.CourseId == courseId);
    }

    public async Task<bool> DeleteForCourseAsync(int courseId)
    {
        var prerequisite = await _context.Prerequisites.FirstOrDefaultAsync(x => x.CourseId == courseId);
        if (prerequisite == null)
            return false;

        _context.Remove(prerequisite);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DoesExistsAsync(int courseId, int prerequisiteCourseId)
    {
        return await _context.Prerequisites
            .AnyAsync(x => x.CourseId == courseId && x.PrerequisiteCourseId == prerequisiteCourseId);
    }
}