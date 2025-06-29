using Applications.DTOs.Semester;
using Applications.Helpers;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using Applications.Utilities;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class SemesterService : ISemesterService
{
    private readonly ISemesterRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;

    public SemesterService(ISemesterRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<SemesterResponse>>> GetListAsync()
    {
        try
        {
            var semesters = await _repository.GetListAsync();
            if (!semesters.Any())
            {
                return Result<IReadOnlyCollection<SemesterResponse>>.Failure("No semesters found", ErrorType.NotFound);
            }

            var response = _mapper.Map<IReadOnlyCollection<SemesterResponse>>(semesters);
            return Result<IReadOnlyCollection<SemesterResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving semesters", ex);
            return Result<IReadOnlyCollection<SemesterResponse>>.Failure(
                "Failed to retrieve semesters", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<SemesterResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<SemesterResponse>.Failure("Invalid semester ID", ErrorType.BadRequest);

        try
        {
            var semester = await _repository.GetByIdAsync(id);
            if (semester == null)
                return Result<SemesterResponse>.Failure("Semester not found", ErrorType.NotFound);

            var response = _mapper.Map<SemesterResponse>(semester);
            return Result<SemesterResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving semester", ex, new { id });
            return Result<SemesterResponse>.Failure("Failed to retrieve semester", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<SemesterResponse>> GetByTermCodeAsync(string termCode)
    {
        if (string.IsNullOrEmpty(termCode))
            return Result<SemesterResponse>.Failure("Term code is required", ErrorType.BadRequest);

        try
        {
            var semester = await _repository.GetByTermCodeAsync(termCode);
            if (semester == null)
                return Result<SemesterResponse>.Failure("Semester not found", ErrorType.NotFound);

            var response = _mapper.Map<SemesterResponse>(semester);
            return Result<SemesterResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving semester", ex, new { termCode });
            return Result<SemesterResponse>.Failure("Failed to retrieve semester", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<SemesterResponse>> AddAsync(SemesterRequest request)
    {
        if (request == default)
            return Result<SemesterResponse>.Failure("Semester data is required", ErrorType.BadRequest);
        
        var result = request.GenerateTermCode(); 
        if(!result.IsSuccess || result.Value == null)
            return Result<SemesterResponse>.Failure(result.Error, result.ErrorType);
        
        try
        {
            var existing = await _repository.GetByTermCodeAsync(result.Value);
            if (existing != null)
                return Result<SemesterResponse>.Failure("Semester with this term code already exists", ErrorType.Conflict);

            var semester = _mapper.Map<Semester>(request);
            semester.IsActive = request.IsActive ?? false;
            semester.TermCode = result.Value;
            
            int id = await _repository.AddAsync(semester);
            if (id <= 0)
                return Result<SemesterResponse>.Failure("Failed to create semester", ErrorType.BadRequest);

            var response = _mapper.Map<SemesterResponse>(semester);
            return Result<SemesterResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error creating semester", ex, new { request });
            return Result<SemesterResponse>.Failure("Failed to create semester", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> UpdateAsync(int id, SemesterRequest request)
    {
        if (request == default)
            return Result.Failure("Semester data is required", ErrorType.BadRequest);

        try
        {
            var existingSemester = await _repository.GetByIdAsync(id);
            if (existingSemester == null)
                return Result.Failure("Semester not found", ErrorType.NotFound);
            
            _mapper.Map(request, existingSemester);
            bool isUpdated = await _repository.UpdateAsync(existingSemester);
            return !isUpdated ? Result.Failure("Failed to update semester", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating semester", ex, new { id, request });
            return Result.Failure("Failed to update semester", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> DeactivateSemesterAsync(int id)
    {
        try
        {
            var semester = await _repository.GetByIdAsync(id);
            if (semester == null)
                return Result.Failure("Semester not found", ErrorType.NotFound);

            semester.IsActive = false;
            bool isUpdated = await _repository.UpdateAsync(semester);
            return !isUpdated ? Result.Failure("Failed to deactivate semester", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error toggling semester status", ex, new { id });
            return Result.Failure("Failed to toggle semester status", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid semester ID", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Semester not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting semester", ex, new { id });
            return Result.Failure("Failed to delete semester", ErrorType.InternalServerError);
        }
    }
}