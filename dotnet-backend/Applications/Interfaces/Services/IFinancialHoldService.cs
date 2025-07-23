using Applications.DTOs.FinancialHold;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IFinancialHoldService
{
    Task<Result<IReadOnlyCollection<FinancialHoldResponse>>> GetListAsync();
    Task<Result<FinancialHoldResponse>> GetByIdAsync(int id);
    Task<Result<IReadOnlyCollection<FinancialHoldResponse>>> GetByStudentIdAsync(int studentId);
    Task<Result<FinancialHoldResponse>> AddAsync(FinancialHoldRequest request);
    Task<Result> UpdateAsync(int id, FinancialHoldRequest request);
    Task<Result> ResolveHoldAsync(int id, ResolveRequest request);
    Task<Result> DeleteAsync(int id);
}