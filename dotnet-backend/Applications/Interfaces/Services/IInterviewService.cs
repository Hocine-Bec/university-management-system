using Applications.DTOs.Interview;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IInterviewService
{
    Task<Result<IReadOnlyCollection<InterviewResponse>>> GetListAsync();
    Task<Result<InterviewResponse>> GetByIdAsync(int id);
    Task<Result<InterviewResponse>> AddAsync(InterviewRequest request);
    Task<Result> UpdateAsync(int id, InterviewRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> CompleteInterviewAsync(int id, CompleteInterviewRequest request);
}