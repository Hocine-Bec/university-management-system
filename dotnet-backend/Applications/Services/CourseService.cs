using Applications.DTOs.Course;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;

    public CourseService(ICourseRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<CourseResponse>>> GetListAsync()
    {
        try
        {
            var courses = await _repository.GetListAsync();
            if (!courses.Any())
                return Result<IReadOnlyCollection<CourseResponse>>.Failure("No courses found", ErrorType.NotFound);

            var response = _mapper.Map<IReadOnlyCollection<CourseResponse>>(courses);
            return Result<IReadOnlyCollection<CourseResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving courses", ex);
            return Result<IReadOnlyCollection<CourseResponse>>.Failure("Failed to retrieve courses", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<CourseResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<CourseResponse>.Failure("Invalid course ID", ErrorType.BadRequest);

        try
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
                return Result<CourseResponse>.Failure("Course not found", ErrorType.NotFound);

            return Result<CourseResponse>.Success(_mapper.Map<CourseResponse>(course));
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving course", ex, new { id });
            return Result<CourseResponse>.Failure("Failed to retrieve course", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<CourseResponse>> GetByCodeAsync(string code)
    {
        if (string.IsNullOrEmpty(code))
            return Result<CourseResponse>.Failure("Course code is required", ErrorType.BadRequest);

        try
        {
            var course = await _repository.GetByCodeAsync(code);
            if (course == null)
                return Result<CourseResponse>.Failure("Course not found", ErrorType.NotFound);

            return Result<CourseResponse>.Success(_mapper.Map<CourseResponse>(course));
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving course", ex, new { code });
            return Result<CourseResponse>.Failure("Failed to retrieve course", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<CourseResponse>> AddAsync(CourseRequest request)
    {
        if (request == default)
            return Result<CourseResponse>.Failure("Course data is required", ErrorType.BadRequest);

        try
        {
            var course = _mapper.Map<Course>(request);
            
            if (await _repository.DoesCodeExistAsync(course.Code))
                return Result<CourseResponse>.Failure("Course code already exists", ErrorType.Conflict);

            int id = await _repository.AddAsync(course);
            if (id <= 0)
                return Result<CourseResponse>.Failure("Failed to create course", ErrorType.BadRequest);

            var response = _mapper.Map<CourseResponse>(course);
            return Result<CourseResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error creating course", ex, new { request });
            return Result<CourseResponse>.Failure(
                "Failed to create course", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, CourseRequest request)
    {
        if (request == default)
            return Result.Failure("Course data is required", ErrorType.BadRequest);

        try
        {
            var existingCourse = await _repository.GetByIdAsync(id);
            if (existingCourse == null)
                return Result.Failure("Course not found", ErrorType.NotFound);

            _mapper.Map(request, existingCourse);

            bool isUpdated = await _repository.UpdateAsync(existingCourse);
            return !isUpdated ? Result.Failure("Failed to update course", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating course", ex, new { id, request });
            return Result.Failure("Failed to update course", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeactivateCourseAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid course ID", ErrorType.BadRequest);

        try
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
                return Result.Failure("Course not found", ErrorType.NotFound);

            course.IsActive = false;
            bool isUpdated = await _repository.UpdateAsync(course);
            
            return !isUpdated ? Result.Failure("Failed to toggle course status", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error toggling course status", ex, new { id });
            return Result.Failure("Failed to toggle course status", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid course ID", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Course not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting course", ex, new { id });
            return Result.Failure("Failed to delete course", ErrorType.InternalServerError);
        }
    }
}