using Applications.DTOs;
using Applications.Helpers;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;
    private readonly IMyLogger _logger;
    private readonly IValidationService _validator;

    public RoleService(IRoleRepository repository, IMyLogger logger, IValidationService validator)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<RoleDto>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<RoleDto>.Failure("Role ID must be a positive integer.", ErrorType.BadRequest);

        try
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null)
                return Result<RoleDto>.Failure("Role not found with the given ID.", ErrorType.NotFound);

            var response = role.ToDto();
            return Result<RoleDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving role by ID", ex, new { id });
            return Result<RoleDto>.Failure("An unexpected error occurred while retrieving the role.", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<RoleDto>> GetByRoleAsync(int roleType)
    {
        if (!Enum.IsDefined(typeof(SystemRole), roleType)) 
            return Result<RoleDto>.Failure("Invalid role value.", ErrorType.BadRequest);

        var role = (SystemRole)roleType;

        try
        {
            var existingRole = await _repository.GetByNameAsync(role);
            if (existingRole == null)
                return Result<RoleDto>.Failure($"Role not found with name '{role.ToString()}'.", ErrorType.NotFound);

            var response = existingRole.ToDto();
            return Result<RoleDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving role by name", ex, new { Role = role.ToString() });
            return Result<RoleDto>.Failure("An unexpected error occurred while retrieving the role.", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<IReadOnlyCollection<RoleDto>>> GetListAsync()
    {
        try
        {
            var roles = await _repository.GetListAsync();
            if (!roles.Any())
                return Result<IReadOnlyCollection<RoleDto>>.Failure("No roles found in the system", ErrorType.NotFound);
            
            var response = roles.Select(ServicesExtension.ToDto).ToList();
            return Result<IReadOnlyCollection<RoleDto>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving all roles", ex);
            return Result<IReadOnlyCollection<RoleDto>>.Failure("An error occurred while fetching all roles.", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<RoleDto>> AddAsync(RoleDto request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsSuccess)
            return Result<RoleDto>.Failure(validationResult.Error, validationResult.ErrorType);

        try
        {
            var role = new Role
            {
                Name = request.Name,
                Description = request.Description
            };

            role.Id = await _repository.AddAsync(role);
            if(role.Id <= 0)
                return Result<RoleDto>.Failure("Failed to add new role", ErrorType.BadRequest);
            
            var response = role.ToDto();
            return Result<RoleDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating new role", ex, request);
            return Result<RoleDto>.Failure("An error occurred while creating the role.", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> UpdateAsync(int id, RoleDto request)
    {
        if (id <= 0)
            return Result.Failure("Role ID must be a positive integer.", ErrorType.BadRequest);

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsSuccess)
            return Result.Failure(validationResult.Error, validationResult.ErrorType);

        try
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null)
                return Result.Failure("Role not found with the given ID.", ErrorType.NotFound);

            role.Name = request.Name;
            role.Description = request.Description;

            bool isUpdated = await _repository.UpdateAsync(role);
            return !isUpdated ? Result.Failure($"Failed to update role", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating role", ex, new { id, request });
            return Result<RoleDto>.Failure("An error occurred while updating the role.", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Role ID must be a positive integer.", ErrorType.BadRequest);
        
        try
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure($"Failed to update role", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting role", ex, new { id });
            return Result<RoleDto>.Failure("An error occurred while deleting the role.", ErrorType.InternalServerError);
        }
    }
}
