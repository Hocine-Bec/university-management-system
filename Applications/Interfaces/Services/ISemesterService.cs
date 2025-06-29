using Applications.DTOs.Semester;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface ISemesterService
{
    Task<Result<IReadOnlyCollection<SemesterResponse>>> GetListAsync();
    Task<Result<SemesterResponse>> GetByIdAsync(int id);
    Task<Result<SemesterResponse>> GetByTermCodeAsync(string termCode);
    Task<Result<SemesterResponse>> AddAsync(SemesterRequest request);
    Task<Result> UpdateAsync(int id, SemesterRequest request);
    Task<Result> DeactivateSemesterAsync(int id);
    Task<Result> DeleteAsync(int id);
}