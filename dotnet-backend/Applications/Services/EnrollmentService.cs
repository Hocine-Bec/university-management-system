using Applications.DTOs.Enrollment;
using Applications.Interfaces.Repositories;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Applications.Helpers;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Services;
using Applications.Interfaces.UnitOfWorks;

namespace Applications.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;

    public EnrollmentService(IUnitOfWork uow, IMapper mapper, IMyLogger logger)
    {
        _uow = uow;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<EnrollmentResponse>>> GetListAsync()
    {
        try
        {
            var enrollments = await _uow.Enrollments.GetListAsync();
            if (!enrollments.Any())
            {
                return Result<IReadOnlyCollection<EnrollmentResponse>>.Failure(
                    "No enrollments found", ErrorType.NotFound);
            }

            var response = _mapper.Map<IReadOnlyCollection<EnrollmentResponse>>(enrollments);
            return Result<IReadOnlyCollection<EnrollmentResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving enrollments", ex);
            return Result<IReadOnlyCollection<EnrollmentResponse>>.Failure(
                "Failed to retrieve enrollments", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<EnrollmentResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<EnrollmentResponse>.Failure("Invalid enrollment ID", ErrorType.BadRequest);

        try
        {
            var enrollment = await _uow.Enrollments.GetByIdAsync(id);
            if (enrollment == null)
                return Result<EnrollmentResponse>.Failure("Enrollment not found", ErrorType.NotFound);

            var response = _mapper.Map<EnrollmentResponse>(enrollment);
            return Result<EnrollmentResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving enrollment", ex, new { id });
            return Result<EnrollmentResponse>.Failure(
                "Failed to retrieve enrollment", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<EnrollmentResponse>> GetByStudentIdAsync(int studentId)
    {
        if (studentId <= 0)
            return Result<EnrollmentResponse>.Failure("Invalid student ID", ErrorType.BadRequest);

        try
        {
            var enrollment = await _uow.Enrollments.GetByStudentIdAsync(studentId);
            if (enrollment == null)
                return Result<EnrollmentResponse>.Failure("Enrollment not found for this student", ErrorType.NotFound);

            var response = _mapper.Map<EnrollmentResponse>(enrollment);
            return Result<EnrollmentResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving enrollment by student", ex, new { studentId });
            return Result<EnrollmentResponse>.Failure(
                "Failed to retrieve enrollment", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<EnrollmentResponse>> AddAsync(EnrollmentRequest request)
    {
        if (request == default)
            return Result<EnrollmentResponse>.Failure("Enrollment data is required", ErrorType.BadRequest);
        
        try
        {
            var canStudentEnroll  = await request.ValidateForEnrollmentAsync(_uow);
        
            if (!canStudentEnroll .IsSuccess)
                return Result<EnrollmentResponse>.Failure(canStudentEnroll.Error, canStudentEnroll.ErrorType);
            
            var enrollment = _mapper.Map<Enrollment>(request);
            enrollment.EnrollmentDate = DateTime.UtcNow;

            int id = await _uow.Enrollments.AddAsync(enrollment);
            if (id <= 0)
                return Result<EnrollmentResponse>.Failure("Failed to create enrollment", ErrorType.BadRequest);

            var response = _mapper.Map<EnrollmentResponse>(enrollment);
            return Result<EnrollmentResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error creating enrollment", ex, new { request });
            return Result<EnrollmentResponse>.Failure("Failed to create enrollment", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, EnrollmentRequest request)
    {
        if (request == default)
            return Result<EnrollmentResponse>.Failure("Enrollment data is required", ErrorType.BadRequest);

        try
        {
            var existingEnrollment = await _uow.Enrollments.GetByIdAsync(id);
            if (existingEnrollment == null)
                return Result.Failure("Enrollment not found", ErrorType.NotFound);

            // Validate relationships if they're being updated
            if (request.StudentId.HasValue || request.ProgramId.HasValue || request.ServiceApplicationId.HasValue)
            {
                var validationResult = await request.ValidatePrerequisitesAsync(_uow);
                
                if (!validationResult.IsSuccess)
                    return Result.Failure(validationResult.Error, validationResult.ErrorType);
            }

            _mapper.Map(request, existingEnrollment);
            existingEnrollment.Id = id;
            bool isUpdated = await _uow.Enrollments.UpdateAsync(existingEnrollment);
            return !isUpdated ? Result.Failure("Failed to update enrollment", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating enrollment", ex, new { id, request });
            return Result.Failure("Failed to update enrollment", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid enrollment ID", ErrorType.BadRequest);
        
        try
        {
            bool isDeleted = await _uow.Enrollments.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Enrollment not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting enrollment", ex, new { id });
            return Result.Failure("Failed to delete enrollment", ErrorType.InternalServerError);
        }
    }
}