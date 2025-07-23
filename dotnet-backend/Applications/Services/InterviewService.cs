using Applications.DTOs.Interview;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class InterviewService : IInterviewService
{
    private readonly IInterviewRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;
    
    public InterviewService(IInterviewRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<Result<IReadOnlyCollection<InterviewResponse>>> GetListAsync()
    {
        try
        {
            var interviews = await _repository.GetListAsync();
            if (!interviews.Any())
                return Result<IReadOnlyCollection<InterviewResponse>>.Failure("No interviews found", ErrorType.NotFound);

            var response = _mapper.Map<IReadOnlyCollection<InterviewResponse>>(interviews);
            return Result<IReadOnlyCollection<InterviewResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving all interviews", ex);
            return Result<IReadOnlyCollection<InterviewResponse>>
                .Failure("Failed to retrieve interviews due to a system error", ErrorType.InternalServerError);
        }
    }
    
    public async Task<Result<InterviewResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<InterviewResponse>.Failure("Invalid interview ID provided", ErrorType.BadRequest);

        try
        {
            var interview = await _repository.GetByIdAsync(id);
            if (interview == null)
                return Result<InterviewResponse>.Failure("Interview not found with the specified ID", ErrorType.NotFound);

            var response = _mapper.Map<InterviewResponse>(interview);
            return Result<InterviewResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving interview", ex, new { id });
            return Result<InterviewResponse>.Failure("Failed to retrieve interview due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<InterviewResponse>> AddAsync(InterviewRequest request)
    {
        if (request == default)
            return Result<InterviewResponse>.Failure("Interview information is required", ErrorType.BadRequest);
        
        try
        {
            var interview = _mapper.Map<Interview>(request);
        
            // Set default fee if not provided
            if (interview.PaidFees == 0)
                interview.PaidFees = 40.00m;
            
            int id = await _repository.AddAsync(interview);
            if (id <= 0)
                return Result<InterviewResponse>.Failure("Failed to create new interview", ErrorType.BadRequest);

            var response = _mapper.Map<InterviewResponse>(interview);
            return Result<InterviewResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error adding new interview", ex, new { request });
            return Result<InterviewResponse>.Failure("Failed to create interview due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, InterviewRequest request)
    {
        if (request == default)
            return Result.Failure("Interview information is required for update", ErrorType.BadRequest);

        try
        {
            var interview = await _repository.GetByIdAsync(id);
            if (interview == null)
                return Result.Failure("Interview not found", ErrorType.NotFound);

            _mapper.Map(request, interview);
            bool isUpdated = await _repository.UpdateAsync(interview);
            return !isUpdated ? Result.Failure("Failed to update interview", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating interview", ex, new { request });
            return Result.Failure("Failed to update interview due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid interview ID provided", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Interview not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting interview", ex, new { id });
            return Result.Failure("Failed to delete interview due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> CompleteInterviewAsync(int id, CompleteInterviewRequest request)
    {
        if (id <= 0)
            return Result.Failure("Invalid interview ID provided", ErrorType.BadRequest);
        
        if (request == default)
            return Result.Failure("Interview information is required", ErrorType.BadRequest);
        
        if (string.IsNullOrWhiteSpace(request.Recommendation))
            return Result.Failure("Recommendation is required", ErrorType.BadRequest);

        try
        {
            var interview = await _repository.GetByIdAsync(id);
            if (interview == null)
                return Result.Failure("Interview not found", ErrorType.NotFound);

            interview.EndTime = request.EndTime;
            interview.IsApproved = request.IsApproved;
            interview.Recommendation = request.Recommendation;
            
            bool isUpdated = await _repository.UpdateAsync(interview);
            return !isUpdated ? Result.Failure("Failed to complete interview", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error completing interview", ex, new { id });
            return Result.Failure("Failed to complete interview due to a system error", ErrorType.InternalServerError);
        }
    }
}