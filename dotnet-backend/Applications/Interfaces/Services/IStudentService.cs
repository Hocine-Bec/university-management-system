using Applications.DTOs.Student;
using Applications.Shared;
using Domain.Enums;

namespace Applications.Interfaces.Services
{
    public interface IStudentService
    {
        Task<Result<IReadOnlyCollection<StudentResponse>>> GetListAsync();
        Task<Result<StudentResponse>> GetByIdAsync(int id);
        Task<Result<StudentResponse>> GetByStudentNumberAsync(string studentNumber);
        Task<Result> DeleteAsync(int id);
        Task<Result> DeleteAsync(string studentNumber);
        Task<Result<StudentResponse>> AddAsync(StudentRequest request);
        Task<Result> UpdateAsync(int id, StudentRequest request);
        Task<Result> UpdateStudentStatusAsync(int id, UpdateStudentStatusRequest request);
    }
}
