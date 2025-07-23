using Applications.DTOs.Enrollment;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IEnrollmentService
{
    Task<Result<IReadOnlyCollection<EnrollmentResponse>>> GetListAsync();
    Task<Result<EnrollmentResponse>> GetByIdAsync(int id);
    Task<Result<EnrollmentResponse>> GetByStudentIdAsync(int studentId);
    Task<Result<EnrollmentResponse>> AddAsync(EnrollmentRequest request);
    Task<Result> UpdateAsync(int id, EnrollmentRequest request);
    Task<Result> DeleteAsync(int id);
}