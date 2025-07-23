using Applications.DTOs.Course;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface ICourseService
{
    Task<Result<IReadOnlyCollection<CourseResponse>>> GetListAsync();
    Task<Result<CourseResponse>> GetByIdAsync(int id);
    Task<Result<CourseResponse>> GetByCodeAsync(string code);
    Task<Result<CourseResponse>> AddAsync(CourseRequest request);
    Task<Result> UpdateAsync(int id, CourseRequest request);
    Task<Result> DeactivateCourseAsync(int id);
    Task<Result> DeleteAsync(int id);
}