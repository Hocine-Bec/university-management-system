using Applications.DTOs.Grade;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class GradeService : IGradeService
{
    private readonly IGradeRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;

    public GradeService(IGradeRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<GradeResponse>>> GetListAsync()
    {
        try
        {
            var grades = await _repository.GetListAsync();
            if (!grades.Any())
                return Result<IReadOnlyCollection<GradeResponse>>.Failure("No grades found", ErrorType.NotFound);

            var response = _mapper.Map<IReadOnlyCollection<GradeResponse>>(grades);
            return Result<IReadOnlyCollection<GradeResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving grades", ex);
            return Result<IReadOnlyCollection<GradeResponse>>.Failure(
                "Failed to retrieve grades", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<GradeResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<GradeResponse>.Failure("Invalid grade ID", ErrorType.BadRequest);

        try
        {
            var grade = await _repository.GetByIdAsync(id);
            if (grade == null)
                return Result<GradeResponse>.Failure("Grade not found", ErrorType.NotFound);

            var response = _mapper.Map<GradeResponse>(grade);
            return Result<GradeResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving grade", ex, new { id });
            return Result<GradeResponse>.Failure(
                "Failed to retrieve grade", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<IReadOnlyCollection<GradeResponse>>> GetByStudentIdAsync(int studentId)
    {
        if (studentId <= 0)
            return Result<IReadOnlyCollection<GradeResponse>>.Failure("Invalid Student ID", ErrorType.BadRequest);

        try
        {
            var grades = await _repository.GetByStudentIdAsync(studentId);
            if (!grades.Any())
                return Result<IReadOnlyCollection<GradeResponse>>.Failure("No grades found for student", ErrorType.NotFound);

            var response = _mapper.Map<IReadOnlyCollection<GradeResponse>>(grades);
            return Result<IReadOnlyCollection<GradeResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving student grades", ex, new { studentId });
            return Result<IReadOnlyCollection<GradeResponse>>.Failure(
                "Failed to retrieve student grades", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<IReadOnlyCollection<GradeResponse>>> GetByCourseIdAsync(int courseId)
    {
        if (courseId <= 0)
            return Result<IReadOnlyCollection<GradeResponse>>.Failure(
                "Invalid course ID", ErrorType.BadRequest);

        try
        {
            var grades = await _repository.GetByCourseIdAsync(courseId);
            if (!grades.Any())
                return Result<IReadOnlyCollection<GradeResponse>>.Failure("No grades found for course", ErrorType.NotFound);

            var response = _mapper.Map<IReadOnlyCollection<GradeResponse>>(grades);
            return Result<IReadOnlyCollection<GradeResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving course grades", ex, new { courseId });
            return Result<IReadOnlyCollection<GradeResponse>>.Failure(
                "Failed to retrieve course grades", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<GradeResponse>> AddAsync(GradeRequest request)
    {
        if (request == default)
            return Result<GradeResponse>.Failure("Grade data required", ErrorType.BadRequest);

        try
        {
            bool exist = await _repository.DoesExistAsync(request.StudentId.Value, request.CourseId.Value, request.SemesterId.Value);
            if(exist)
                return Result<GradeResponse>.Failure("Grade already exists", ErrorType.Conflict);
            
            var grade = _mapper.Map<Grade>(request);
            var id = await _repository.AddAsync(grade);
            
            if (id <= 0)
                return Result<GradeResponse>.Failure("Failed to create grade", ErrorType.BadRequest);

            grade = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<GradeResponse>(grade);
            return Result<GradeResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating grade", ex, new { request });
            return Result<GradeResponse>.Failure("Failed to create grade", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, GradeRequest request)
    {
        if (request == default)
            return Result.Failure("Grade data required", ErrorType.BadRequest);

        try
        {
            var grade = await _repository.GetByIdAsync(id);
            if (grade == null)
                return Result.Failure("Grade not found", ErrorType.NotFound);

            _mapper.Map(request, grade);
            var isUpdated = await _repository.UpdateAsync(grade);
            return isUpdated ? Result.Success : Result.Failure("Grade update failed", ErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating grade", ex, new { id, request });
            return Result.Failure("Failed to update grade", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid grade ID", ErrorType.BadRequest);

        try
        {
            var isDeleted = await _repository.DeleteAsync(id);
            return isDeleted ? Result.Success : Result.Failure("Grade not found", ErrorType.NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting grade", ex, new { id });
            return Result.Failure("Failed to delete grade", ErrorType.InternalServerError);
        }
    }
}