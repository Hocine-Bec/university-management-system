using Applications.DTOs.Prerequisite;
using Applications.Helpers;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Services;
using Applications.Interfaces.UnitOfWorks;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using static Applications.Shared.Result;

public class PrerequisiteService : IPrerequisiteService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;

    public PrerequisiteService(IUnitOfWork uow, IMapper mapper, IMyLogger logger)
    {
        _uow = uow;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PrerequisiteResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<PrerequisiteResponse>.Failure("Invalid prerequisite ID", ErrorType.BadRequest);

        try
        {
            var prerequisites = await _uow.Prerequisites.GetByIdAsync(id);
            if(prerequisites == null)
                return Result<PrerequisiteResponse>.Failure("Prerequisite not found", ErrorType.NotFound);
            
            var response = _mapper.Map<PrerequisiteResponse>(prerequisites);
            return Result<PrerequisiteResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving prerequisites", ex, new { id });
            return Result<PrerequisiteResponse>.Failure("Failed to retrieve prerequisites", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<IReadOnlyCollection<PrerequisiteResponse>>> GetByCourseIdAsync(int courseId)
    {
        if (courseId <= 0)
            return Result<IReadOnlyCollection<PrerequisiteResponse>>.Failure("Invalid course ID", ErrorType.BadRequest);

        try
        {
            var prerequisites = await _uow.Prerequisites.GetByCourseIdAsync(courseId);
            if(prerequisites == null)
                return Result<IReadOnlyCollection<PrerequisiteResponse>>.Failure("Prerequisite not found", ErrorType.NotFound);
            
            var response = _mapper.Map<IReadOnlyCollection<PrerequisiteResponse>>(prerequisites);
            return Result<IReadOnlyCollection<PrerequisiteResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving prerequisites", ex, new { courseId });
            return Result<IReadOnlyCollection<PrerequisiteResponse>>.Failure(
                "Failed to retrieve prerequisites", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<PrerequisiteResponse>> AddAsync(PrerequisiteRequest request)
    {
        if (request == default)
            return Result<PrerequisiteResponse>.Failure("Prerequisite data is required", ErrorType.BadRequest);
        
        try
        {
            var result = await request.ValidateForCourseAsync(_uow);
            if (!result.IsSuccess)
                return Result<PrerequisiteResponse>.Failure(result.Error, result.ErrorType);
            
            var prerequisite = _mapper.Map<Prerequisite>(request);
            int id = await _uow.Prerequisites.AddAsync(prerequisite);
            
            if (id <= 0)
                return Result<PrerequisiteResponse>.Failure("Failed to add prerequisite", ErrorType.BadRequest);

            var response = _mapper.Map<PrerequisiteResponse>(prerequisite);
            return Result<PrerequisiteResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error adding prerequisite", ex, request);
            return Result<PrerequisiteResponse>.Failure("Failed to add prerequisite", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> UpdateAsync(int id, PrerequisiteRequest request)
    {
        if (id <= 0)
            return Failure("Invalid prerequisite ID", ErrorType.BadRequest);

        if (request == default)
            return Result.Failure("Prerequisite data is required", ErrorType.BadRequest);
        try
        {
            var result = await request.ValidateForCourseAsync(_uow);
            if (!result.IsSuccess)
                return Failure(result.Error, result.ErrorType);
            
            var existing = await _uow.Prerequisites.GetByIdAsync(id);
            if (existing == null)
                return Failure("Prerequisite not found", ErrorType.NotFound);

            _mapper.Map(request, existing);
            bool updated = await _uow.Prerequisites.UpdateAsync(existing);
            
            return updated ? Success : Failure("Failed to update prerequisite", ErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating prerequisite", ex, new { id, request });
            return Failure("Failed to update prerequisite", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Failure("Invalid prerequisite ID", ErrorType.BadRequest);

        try
        {
            bool deleted = await _uow.Prerequisites.DeleteAsync(id);
            return deleted ? Success : Failure("Prerequisite not found", ErrorType.NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting prerequisite", ex, new { id });
            return Failure("Failed to delete prerequisite", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> DeleteForCourseAsync(int courseId)
    {
        if (courseId <= 0)
            return Failure("Invalid course ID", ErrorType.BadRequest);

        try
        {
            bool deleted = await _uow.Prerequisites.DeleteForCourseAsync(courseId);
            return deleted ? Success : Failure("No prerequisites found", ErrorType.NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting course prerequisites", ex, new { courseId });
            return Failure("Failed to delete prerequisites", ErrorType.InternalServerError);
        }
    }
}