using Applications.DTOs.Grade;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IGradeService
{
    Task<Result<IReadOnlyCollection<GradeResponse>>> GetListAsync();
    Task<Result<GradeResponse>> GetByIdAsync(int id);
    Task<Result<IReadOnlyCollection<GradeResponse>>> GetByStudentIdAsync(int studentId);
    Task<Result<IReadOnlyCollection<GradeResponse>>> GetByCourseIdAsync(int courseId);
    Task<Result<GradeResponse>> AddAsync(GradeRequest request);
    Task<Result> UpdateAsync(int id, GradeRequest request);
    Task<Result> DeleteAsync(int id);
}