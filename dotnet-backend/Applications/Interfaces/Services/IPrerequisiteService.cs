using Applications.DTOs.Prerequisite;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IPrerequisiteService
{
    Task<Result<IReadOnlyCollection<PrerequisiteResponse>>> GetByCourseIdAsync(int courseId);
    Task<Result<PrerequisiteResponse>> GetByIdAsync(int id);
    Task<Result<PrerequisiteResponse>> AddAsync(PrerequisiteRequest request);
    Task<Result> UpdateAsync(int id, PrerequisiteRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> DeleteForCourseAsync(int courseId);
}