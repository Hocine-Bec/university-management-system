using Applications.DTOs.EntranceExam;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class EntranceExamService : IEntranceExamService
{
    private readonly IEntranceExamRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;
    
    public EntranceExamService(IEntranceExamRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<Result<IReadOnlyCollection<EntranceExamResponse>>> GetListAsync()
    {
        try
        {
            var exams = await _repository.GetListAsync();
            if (!exams.Any())
            {
                return Result<IReadOnlyCollection<EntranceExamResponse>>.Failure(
                    "No entrance exams found", ErrorType.NotFound);
            }

            var response = _mapper.Map<IReadOnlyCollection<EntranceExamResponse>>(exams);
            return Result<IReadOnlyCollection<EntranceExamResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving all entrance exams", ex);
            return Result<IReadOnlyCollection<EntranceExamResponse>>
                .Failure("Failed to retrieve entrance exams due to a system error", ErrorType.InternalServerError);
        }
    }
    
    public async Task<Result<EntranceExamResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<EntranceExamResponse>.Failure("Invalid entrance exam ID provided", ErrorType.BadRequest);

        try
        {
            var exam = await _repository.GetByIdAsync(id);
            if (exam == null)
                return Result<EntranceExamResponse>.Failure("Entrance exam not found with the specified ID", ErrorType.NotFound);

            var response = _mapper.Map<EntranceExamResponse>(exam);
            return Result<EntranceExamResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving entrance exam", ex, new { id });
            return Result<EntranceExamResponse>.Failure("Failed to retrieve entrance exam due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<EntranceExamResponse>> AddAsync(EntranceExamRequest request)
    {
        if (request == default)
            return Result<EntranceExamResponse>.Failure("Entrance exam information is required", ErrorType.BadRequest);
        
        try
        {
            var exam = _mapper.Map<EntranceExam>(request);
            
            int id = await _repository.AddAsync(exam);
            if (id <= 0)
                return Result<EntranceExamResponse>.Failure("Failed to create new entrance exam", ErrorType.BadRequest);
            
            var response = _mapper.Map<EntranceExamResponse>(exam);
            return Result<EntranceExamResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error adding new entrance exam", ex, new { request });
            return Result<EntranceExamResponse>.Failure("Failed to create entrance exam due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, EntranceExamRequest request)
    {
        if (request == default)
            return Result.Failure("Entrance exam information is required", ErrorType.BadRequest);

        try
        {
            var exam = await _repository.GetByIdAsync(id);
            if (exam == null)
                return Result.Failure("Entrance exam not found", ErrorType.NotFound);

            _mapper.Map(request, exam);
            
            if (exam.Score.HasValue && exam.ExamStatus != ExamStatus.Scored)
            {
                exam.ExamStatus = ExamStatus.Scored;
                exam.IsPassed = exam.Score >= exam.PassingScore;
            }
       
            bool isUpdated = await _repository.UpdateAsync(exam);
            return !isUpdated ? Result.Failure("Failed to update entrance exam", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating entrance exam", ex, new { request });
            return Result.Failure("Failed to update entrance exam due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid entrance exam ID provided", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Entrance exam not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting entrance exam", ex, new { id });
            return Result.Failure("Failed to delete entrance exam due to a system error", ErrorType.InternalServerError);
        }
    }
    
    public async Task<Result> UpdateScoreCriteriaAsync(int id, UpdateScoreCriteriaRequest request)
    {
        if (id <= 0)
            return Result.Failure("Invalid entrance exam ID provided", ErrorType.BadRequest);
     
        if(request == default)
            return Result.Failure("Score data is required", ErrorType.BadRequest);
        
        if (request.MaxScore <= 0)
            return Result.Failure("Invalid max score provided", ErrorType.BadRequest);
        
        if (request.PassingScore <= 0)
            return Result.Failure("Invalid passing score provided", ErrorType.BadRequest);

        try
        {
            var exam = await _repository.GetByIdAsync(id);
            if (exam == null)
                return Result.Failure("Entrance exam not found", ErrorType.NotFound);

            exam.MaxScore = request.MaxScore;
            exam.PassingScore = request.PassingScore;
            
            bool isUpdated = await _repository.UpdateAsync(exam);
            return !isUpdated ? Result.Failure("Failed to update entrance exam score", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating entrance exam score", ex, new { id, request });
            return Result.Failure("Failed to update entrance exam score due to a system error", ErrorType.InternalServerError);
        }
    }
    
    public async Task<Result> UpdateStudentScoreAsync(int id, UpdateScoreRequest request)
    {
        if (id <= 0)
            return Result.Failure("Invalid entrance exam ID provided", ErrorType.BadRequest);
     
        if(request == default)
            return Result.Failure("Score data is required", ErrorType.BadRequest);
        
        if (request.Score is < 0 or > 100)
            return Result.Failure("Score must be between 0 and 100", ErrorType.BadRequest);

        try
        {
            var exam = await _repository.GetByIdAsync(id);
            if (exam == null)
                return Result.Failure("Entrance exam not found", ErrorType.NotFound);

            exam.Score = request.Score;
            exam.ExamStatus = ExamStatus.Scored;
            exam.IsPassed = request.Score >= exam.PassingScore;
            exam.Notes = request.Notes;
            
            bool isUpdated = await _repository.UpdateAsync(exam);
            return !isUpdated ? Result.Failure("Failed to update entrance exam score", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating entrance exam score", ex, new { id, request.Score });
            return Result.Failure("Failed to update entrance exam score due to a system error", ErrorType.InternalServerError);
        }
    }
    
    public async Task<Result> CancelExamAsync(int id, string? notes)
    {
        if (id <= 0)
            return Result.Failure("Invalid entrance exam ID provided", ErrorType.BadRequest);
     
        try
        {
            var exam = await _repository.GetByIdAsync(id);
            if (exam == null)
                return Result.Failure("Entrance exam not found", ErrorType.NotFound);

            exam.ExamStatus = ExamStatus.Cancelled;
            exam.PaidFees = 0;
            exam.Notes = notes;
            exam.IsPassed = null;
            exam.Score = null;
            
            bool isUpdated = await _repository.UpdateAsync(exam);
            return !isUpdated ? Result.Failure("Failed to update entrance exam score", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating entrance exam score", ex, new { id, notes });
            return Result.Failure("Failed to update entrance exam score due to a system error", ErrorType.InternalServerError);
        }
    }
}