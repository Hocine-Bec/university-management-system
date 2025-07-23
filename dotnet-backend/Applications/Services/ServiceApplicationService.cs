using Applications.DTOs.ServiceApplication;
using Applications.Helpers;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class ServiceApplicationService : IServiceApplicationService
{
    private readonly IServiceApplicationRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;

    public ServiceApplicationService(IServiceApplicationRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<ServiceApplicationResponse>>> GetListAsync()
    {
        try
        {
            var applications = await _repository.GetListAsync();
            if (!applications.Any())
            {
                return Result<IReadOnlyCollection<ServiceApplicationResponse>>.Failure(
                    "No service applications found", ErrorType.NotFound);
            }

            var response = _mapper.Map<IReadOnlyCollection<ServiceApplicationResponse>>(applications);
            return Result<IReadOnlyCollection<ServiceApplicationResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving service applications", ex);
            return Result<IReadOnlyCollection<ServiceApplicationResponse>>.Failure(
                "Failed to retrieve service applications", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<ServiceApplicationResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<ServiceApplicationResponse>.Failure("Invalid application ID", ErrorType.BadRequest);

        try
        {
            var application = await _repository.GetByIdAsync(id);
            if (application == null)
                return Result<ServiceApplicationResponse>.Failure("Service application not found", ErrorType.NotFound);

            var response = _mapper.Map<ServiceApplicationResponse>(application);
            return Result<ServiceApplicationResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving service application", ex, new { id });
            return Result<ServiceApplicationResponse>.Failure(
                "Failed to retrieve service application", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<ServiceApplicationResponse>> GetByPersonIdAsync(int personId)
    {
        if (personId <= 0)
            return Result<ServiceApplicationResponse>.Failure("Invalid person ID", ErrorType.BadRequest);

        try
        {
            var application = await _repository.GetByPersonIdAsync(personId);
            if (application == null)
                return Result<ServiceApplicationResponse>.Failure("Service application not found", ErrorType.NotFound);

            var response = _mapper.Map<ServiceApplicationResponse>(application);
            return Result<ServiceApplicationResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving service application", ex, new { personId });
            return Result<ServiceApplicationResponse>.Failure(
                "Failed to retrieve service application", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<ServiceApplicationResponse>> AddAsync(ServiceApplicationCreateRequest request)
    {
        if (request == default)
            return Result<ServiceApplicationResponse>.Failure("Application data is required", ErrorType.BadRequest);
        
        try
        {
            var result = await _repository.DoesPersonHaveActiveApplicationsAsync(request.PersonId, request.ServiceOfferId);
            if(result)
                return Result<ServiceApplicationResponse>.Failure(
                    "Person already has an active/incomplete application for this service type", ErrorType.Conflict);
            
            var application = _mapper.Map<ServiceApplication>(request);
            application.ApplicationDate = DateTime.UtcNow;
            application.Status = ApplicationStatus.New;

            int id = await _repository.AddAsync(application);
            if (id <= 0)
                return Result<ServiceApplicationResponse>.Failure("Failed to create application", ErrorType.BadRequest);

            var response = _mapper.Map<ServiceApplicationResponse>(application);
            return Result<ServiceApplicationResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error creating service application", ex, new { request });
            return Result<ServiceApplicationResponse>.Failure(
                "Failed to create service application", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, ServiceApplicationUpdateRequest request)
    {
        if (request == default)
            return Result.Failure("Application data is required", ErrorType.BadRequest);

        try
        {
            var existingApp = await _repository.GetByIdAsync(id);
            if (existingApp == null)
                return Result.Failure("Application not found", ErrorType.NotFound);

            _mapper.Map(request, existingApp);
            bool isUpdated = await _repository.UpdateAsync(existingApp);
            return !isUpdated ? Result.Failure("Failed to update application", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating service application", ex, new { id, request });
            return Result.Failure("Failed to update application", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateStatusAsync(int id, ServiceApplicationUpdateStatusRequest request)
    {
        if(request == default || !request.Status.HasValue)
            return Result.Failure("Status information is required", ErrorType.BadRequest);
        try
        {
            var application = await _repository.GetByIdAsync(id);
            if (application == null)
                return Result.Failure("Application not found", ErrorType.NotFound);

            application.Status = request.Status.Value;
            bool isUpdated = await _repository.UpdateAsync(application);
            return !isUpdated ? Result.Failure("Failed to update status", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating application status", ex, new { id, status = request });
            return Result.Failure("Failed to update status", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid application ID", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Application not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting service application", ex, new { id });
            return Result.Failure("Failed to delete application", ErrorType.InternalServerError);
        }
    }
}