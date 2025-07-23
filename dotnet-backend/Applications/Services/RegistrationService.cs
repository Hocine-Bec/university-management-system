using Applications.DTOs.Registration;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;

    public RegistrationService(IRegistrationRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<RegistrationResponse>>> GetListAsync()
    {
        try
        {
            var registrations = await _repository.GetListAsync();
            if (!registrations.Any())
                return Result<IReadOnlyCollection<RegistrationResponse>>.Failure("No registrations found", ErrorType.NotFound);

            var response = _mapper.Map<IReadOnlyCollection<RegistrationResponse>>(registrations);
            return Result<IReadOnlyCollection<RegistrationResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving registrations", ex);
            return Result<IReadOnlyCollection<RegistrationResponse>>.Failure(
                "Failed to retrieve registrations", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<RegistrationResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<RegistrationResponse>.Failure("Invalid ID", ErrorType.BadRequest);

        try
        {
            var registration = await _repository.GetByIdAsync(id);
            if (registration == null)
                return Result<RegistrationResponse>.Failure("Registration not found", ErrorType.NotFound);

            var response = _mapper.Map<RegistrationResponse>(registration);
            return Result<RegistrationResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving registration", ex, new { id });
            return Result<RegistrationResponse>.Failure(
                "Failed to retrieve registration", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<RegistrationResponse>> AddAsync(RegistrationRequest request)
    {
        if(request == default)
            return Result<RegistrationResponse>.Failure("Registration data is required", ErrorType.BadRequest);
        
        try
        {
            var exists = await _repository.DoesExistAsync(
                request.StudentId.Value, request.SectionId.Value, request.SemesterId.Value);
            
            if (exists)
                return Result<RegistrationResponse>.Failure("Registration already exists", ErrorType.Conflict);

            var registration = _mapper.Map<Registration>(request);
            registration.RegistrationDate = DateTime.UtcNow;

            var id = await _repository.AddAsync(registration);
            if (id <= 0)
                return Result<RegistrationResponse>.Failure("Failed to create registration", ErrorType.BadRequest);

            registration = await _repository.GetByIdAsync(id);
            var response = _mapper.Map<RegistrationResponse>(registration);
            return Result<RegistrationResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating registration", ex, new { request });
            return Result<RegistrationResponse>.Failure(
                "Failed to create registration", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, RegistrationRequest request)
    {
        if(request == default)
            return Result.Failure("Registration data is required", ErrorType.BadRequest);
        
        try
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return Result.Failure("Registration not found", ErrorType.NotFound);

            _mapper.Map(request, existing);
            var isUpdated = await _repository.UpdateAsync(existing);
            return isUpdated ? Result.Success : Result.Failure("Failed to update registration", ErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating registration", ex, new { id, request });
            return Result.Failure("Failed to update registration", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if(id <= 0)
            return Result.Failure("Invalid Registration ID", ErrorType.BadRequest);
        
        try
        {
            var isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Registration not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting registration", ex, new { id });
            return Result.Failure("Failed to delete registration", ErrorType.InternalServerError);
        }
    }
}