using Applications.DTOs.Users;
using Applications.Interfaces.Auth;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _hasher;
        private readonly IMapper _mapper;
        private readonly IMyLogger _logger;

        public UserService(IUserRepository repository, IMyLogger logger, IMapper mapper, IPasswordHasher hasher)
        {
            _repository = repository;
            _mapper = mapper;
            _hasher = hasher;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyCollection<UserResponse>>> GetListAsync()
        {
            try
            {
                //Refactor later to use AutoMapper Projection when the database grows
                var users = await _repository.GetListAsync();
                if (!users.Any())
                {
                    return Result<IReadOnlyCollection<UserResponse>>.Failure(
                        "No users found in the system", ErrorType.NotFound);
                }

                var response = _mapper.Map<IReadOnlyCollection<UserResponse>>(users);
                return Result<IReadOnlyCollection<UserResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving all users", ex);

                return Result<IReadOnlyCollection<UserResponse>>
                    .Failure("Failed to retrieve users due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result<UserResponse>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return Result<UserResponse>.Failure("Invalid user ID provided", ErrorType.BadRequest);

            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                    return Result<UserResponse>.Failure("User not found with the specified ID", ErrorType.NotFound);

                var response = _mapper.Map<UserResponse>(user);
                return Result<UserResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving user", ex, new { id });
                return Result<UserResponse>.Failure("Failed to retrieve user due to a system error",
                    ErrorType.InternalServerError);
            }
        }
        public async Task<Result<UserResponse>> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                return Result<UserResponse>.Failure("Username is required", ErrorType.BadRequest);

            try
            {
                var student = await _repository.GetByUsernameAsync(username);
                if (student == null)
                    return Result<UserResponse>.Failure("User not found with the specified username", ErrorType.NotFound);

                var response = _mapper.Map<UserResponse>(student);
                return Result<UserResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving users", ex, new { username });
                return Result<UserResponse>.Failure("Failed to retrieve users due to a system error",
                    ErrorType.InternalServerError);
            }
        }
        public async Task<Result<UserResponse>> AddAsync(CreateUserRequest request)
        {
            if (request == default)
                return Result<UserResponse>.Failure("User information is required", ErrorType.BadRequest);
            
            try
            {
                var isExist = await _repository.DoesExistAsync(request.PersonId);
                if (isExist)
                    return Result<UserResponse>.Failure("User already exists", ErrorType.Conflict);

                var user = _mapper.Map<User>(request);
                user.Password = _hasher.HashPassword(request.Password);
                
                int id = await _repository.AddAsync(user);
                if (id <= 0)
                    return Result<UserResponse>.Failure("Failed to create new user record", ErrorType.BadRequest);

                var response = _mapper.Map<UserResponse>(user);
                return Result<UserResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error adding new user", ex, new { request });
                return Result<UserResponse>.Failure("Failed to create user due to a system error",
                    ErrorType.InternalServerError);
            }
        }
        public async Task<Result> DeleteAsync(int id)
        {
            if (id <= 0)
                return Result.Failure("Invalid user ID provided", ErrorType.BadRequest);

            try
            {
                bool isDeleted = await _repository.DeleteAsync(id);
                return !isDeleted ? Result.Failure("User not found", ErrorType.BadRequest) : Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error deleting user", ex, new { id });
                return Result.Failure("Failed to delete user due to a system error", ErrorType.InternalServerError);
            }
        }
        public async Task<Result> DeleteAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                return Result.Failure("Username is required", ErrorType.BadRequest);

            try
            {
                bool isDeleted = await _repository.DeleteAsync(username);
                return !isDeleted ? Result.Failure("Username not found", ErrorType.BadRequest) : Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error deleting user", ex, new { username });
                return Result.Failure("Failed to delete user due to a system error", ErrorType.InternalServerError);
            }
        }
        public async Task<Result> UpdateAsync(int id, UpdateUserRequest request)
        {
            if (request == default)
                return Result.Failure("User information is required for update", ErrorType.BadRequest);
            
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                    return Result.Failure("User not found", ErrorType.NotFound);

                _mapper.Map(request, user);
                
                bool isUpdated = await _repository.UpdateAsync(user);
                return !isUpdated ? Result.Failure($"Failed to update user", ErrorType.BadRequest) : Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error updating user", ex, new { request });
                return Result.Failure("Failed to update user due to a system error", ErrorType.InternalServerError);
            }
        }
        public async Task<Result> ChangePasswordAsync(int id, ChangePasswordRequest request)
        {
            if (id <= 0)
                return Result.Failure("Invalid user ID provided", ErrorType.BadRequest);

            if (request == default)
                return Result.Failure("Password information is required for update", ErrorType.BadRequest);
            
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                    return Result<UserResponse>.Failure("User not found with the specified ID", ErrorType.NotFound);

                bool isCorrectPassword = _hasher.VerifyPassword(request.CurrentPassword, user.Password);
                if(!isCorrectPassword)
                    return Result.Failure("Incorrect current password", ErrorType.BadRequest);

                user.Password = _hasher.HashPassword(request.NewPassword);
                user.LastLoginAt = DateTime.UtcNow;
                
                bool isUpdated = await _repository.UpdateAsync(user);
                return !isUpdated ? Result.Failure($"Failed to update user password", ErrorType.BadRequest) : Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error updating user password", ex, new { request });
                return Result.Failure("Failed to update user password due to a system error", ErrorType.InternalServerError);
            }
        }
        public async Task<Result<IReadOnlyCollection<UserResponse>>> GetByRoleAsync(int roleId)
        {
            if (roleId <= 0)
                return Result<IReadOnlyCollection<UserResponse>>.Failure("Invalid role ID", ErrorType.BadRequest);

            try
            {
                var users = await _repository.GetByRoleAsync(roleId);
                if (!users.Any())
                    return Result<IReadOnlyCollection<UserResponse>>.Failure("No users found", ErrorType.NotFound);
                
                var response = _mapper.Map<IReadOnlyCollection<UserResponse>>(users);
                return Result<IReadOnlyCollection<UserResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving users by role", ex, new { roleId });
                return Result<IReadOnlyCollection<UserResponse>>.Failure("Failed to retrieve users", ErrorType.InternalServerError);
            }
        }
    }
}
