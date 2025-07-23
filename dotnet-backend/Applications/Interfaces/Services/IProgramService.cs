using Applications.DTOs.Program;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IProgramService 
{
    Task<Result<IReadOnlyCollection<ProgramResponse>>> GetListAsync();
    Task<Result<ProgramResponse>> GetByIdAsync(int id);
    Task<Result<ProgramResponse>> GetByCodeAsync(string code);
    Task<Result<ProgramResponse>> AddAsync(ProgramRequest request);
    Task<Result> UpdateAsync(int id, ProgramRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> DeleteAsync(string code);
}