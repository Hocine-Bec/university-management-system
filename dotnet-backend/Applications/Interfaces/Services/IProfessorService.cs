using Applications.DTOs.Professor;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IProfessorService
{
    Task<Result<IReadOnlyCollection<ProfessorResponse>>> GetListAsync();
    Task<Result<ProfessorResponse>> GetByIdAsync(int id);
    Task<Result<ProfessorResponse>> GetByEmployeeNumberAsync(string employeeNumber);
    Task<Result<ProfessorResponse>> AddAsync(ProfessorRequest request);
    Task<Result> UpdateAsync(int id, ProfessorRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> DeleteAsync(string employeeNumber);
}