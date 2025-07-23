using Applications.DTOs.EntranceExam;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IEntranceExamService
{
    Task<Result<IReadOnlyCollection<EntranceExamResponse>>> GetListAsync();
    Task<Result<EntranceExamResponse>> GetByIdAsync(int id);
    Task<Result<EntranceExamResponse>> AddAsync(EntranceExamRequest request);
    Task<Result> UpdateAsync(int id, EntranceExamRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> UpdateStudentScoreAsync(int id, UpdateScoreRequest score);
    Task<Result> UpdateScoreCriteriaAsync(int id, UpdateScoreCriteriaRequest request);
    Task<Result> CancelExamAsync(int id, string? notes);
}