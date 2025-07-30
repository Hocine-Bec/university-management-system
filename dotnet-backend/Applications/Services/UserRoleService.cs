using Applications.DTOs.UserRole;
using Applications.Helpers;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using Domain.Enums;

namespace Applications.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository _repository;
    private readonly IMyLogger _logger;
    private readonly IValidationService _validator;

    public UserRoleService(IUserRoleRepository repository, IMyLogger logger, IValidationService validator)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<UserRoleDto>> AssignRoleAsync(UserRoleDto request)
    {
        if (request.UserId <= 0 || request.RoleId <= 0)
            return Result<UserRoleDto>.Failure("User ID and Role ID must be greater than zero.", ErrorType.BadRequest);

        var userRole = request.FromDto();

        try
        {
            var existingAssignment = await _repository.GetByUserAndRoleAsync(userRole);
            if (existingAssignment != null)
            {
                existingAssignment.IsActive = true;
                await _repository.UpdateAsync(existingAssignment);
                return Result<UserRoleDto>.Success(userRole.ToDto());
            }

            userRole.IsActive = true;
            var response = await _repository.AssignRoleAsync(userRole);
            return Result<UserRoleDto>.Success(response.ToDto());
        }
        catch (Exception ex)
        {
            _logger.LogError("Error assigning role to user", ex, new { userRole });
            return Result<UserRoleDto>.Failure("An unexpected error occurred while assigning the role to the user.", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> RemoveRoleAsync(UserRoleDto request)
    {
        if (request.UserId <= 0 || request.RoleId <= 0)
            return Result<UserRoleDto>.Failure("User ID and Role ID must be greater than zero.", ErrorType.BadRequest);

        var userRole = request.FromDto();
        
        try
        {
            var role = await _repository.GetByUserAndRoleAsync(userRole);
            if (role == null)
                return Result.Failure("The specified role is not assigned to the user.", ErrorType.NotFound);

            role.IsActive = false;
            await _repository.UpdateAsync(role);
            return Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error removing role from user", ex, new { userRole });
            return Result.Failure("An error occurred while trying to remove the role from the user.", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<bool>> UserHasRoleAsync(UserRoleDto request)
    {
        if (request.UserId <= 0 || request.RoleId <= 0)
            return Result<bool>.Failure("User ID and Role ID must be greater than zero.", ErrorType.BadRequest);

        var userRole = request.FromDto();
        
        try
        {
            bool hasRole = await _repository.UserHasRoleAsync(userRole);
            return !hasRole
                ? Result<bool>.Failure("The user does not have the specified role.", ErrorType.NotFound)
                : Result<bool>.Success(hasRole);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error checking user role", ex, new { userRole });
            return Result<bool>.Failure("An unexpected error occurred while checking the user's role.", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<bool>> UserHasRoleAsync(int userId, int roleType)
    {
        if (userId <= 0)
            return Result<bool>.Failure("User ID must be greater than zero", ErrorType.BadRequest);
        
        if (!Enum.IsDefined(typeof(SystemRole), roleType))
            return Result<bool>.Failure("Role must be specified.", ErrorType.BadRequest);
        
        var role = (SystemRole)roleType;
        
        try
        {
            bool hasRole = await _repository.UserHasRoleAsync(userId, role);
            return !hasRole
                ? Result<bool>.Failure("The user does not have the specified role.", ErrorType.NotFound)
                : Result<bool>.Success(hasRole);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error checking user role by enum", ex, new { userId, Role = role.ToString() });
            return Result<bool>.Failure("An unexpected error occurred while checking the user's role by name.", 
                ErrorType.InternalServerError);
        }
    }
    public async Task<Result<UserRoleDto>> GetByUserAndRoleAsync(UserRoleDto request)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsSuccess)
            return Result<UserRoleDto>.Failure(validationResult.Error, validationResult.ErrorType);

        var userRole = request.FromDto();

        try
        {
            userRole = await _repository.GetByUserAndRoleAsync(userRole);
            if (userRole == null)
                return Result<UserRoleDto>.Failure("No role assignment found for the specified user and role.", ErrorType.NotFound);

            var response = userRole.ToDto();
            return Result<UserRoleDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving user-role mapping", ex, new { userRole });
            return Result<UserRoleDto>.Failure("Failed to retrieve the user-role mapping due to an internal error.", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<IReadOnlyCollection<UserRoleDto>>> GetByUserIdAsync(int userId)
    {
        if (userId <= 0)
            return Result<IReadOnlyCollection<UserRoleDto>>.Failure("User ID must be a positive integer.", ErrorType.BadRequest);

        try
        {
            var roles = await _repository.GetByUserIdAsync(userId);
            if (!roles.Any())
            {
                return Result<IReadOnlyCollection<UserRoleDto>>.Failure(
                    "No roles are currently assigned to this user.", ErrorType.BadRequest);
            }

            var response = roles.Select(x => x.ToDto()).ToList();
            return Result<IReadOnlyCollection<UserRoleDto>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving roles by user ID", ex, new { userId });
            return Result<IReadOnlyCollection<UserRoleDto>>.Failure("An error occurred while retrieving roles for the specified user.", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<IReadOnlyCollection<UserRoleDto>>> GetByRoleIdAsync(int roleId)
    {
        if (roleId <= 0)
            return Result<IReadOnlyCollection<UserRoleDto>>.Failure("Role ID must be a positive integer.", ErrorType.BadRequest);

        try
        {
            var roles = await _repository.GetByRoleIdAsync(roleId);
            if (!roles.Any())
            {
                return Result<IReadOnlyCollection<UserRoleDto>>.Failure(
                    "No users are currently assigned to this role.", ErrorType.BadRequest);
            }

            var response = roles.Select(x => x.ToDto()).ToList();
            return Result<IReadOnlyCollection<UserRoleDto>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving users by role ID", ex, new { roleId });
            return Result<IReadOnlyCollection<UserRoleDto>>.Failure("An internal error occurred while retrieving users by role.", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<IReadOnlyCollection<RoleNameResponse>>> GetUserRoleNamesAsync(int userId)
    {
        if (userId <= 0)
        {
            return Result<IReadOnlyCollection<RoleNameResponse>>.Failure(
                "User ID must be a positive integer.", ErrorType.BadRequest);
        }

        try
        {
            var roles = await _repository.GetUserRoleNamesAsync(userId);
            if (!roles.Any())
            {
                return Result<IReadOnlyCollection<RoleNameResponse>>.Failure("", ErrorType.NotFound);
            }

            var response = roles.Select(x => new RoleNameResponse { RoleName = x.ToString() }).ToList();
            
            return !roles.Any()
                ? Result<IReadOnlyCollection<RoleNameResponse>>.Failure("No valid roles found for the specified user.", ErrorType.NotFound)
                : Result<IReadOnlyCollection<RoleNameResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving user role names", ex, new { userId });
            return Result<IReadOnlyCollection<RoleNameResponse>>.Failure(
                "An unexpected error occurred while retrieving user role names.", ErrorType.InternalServerError);
        }
    }
}

