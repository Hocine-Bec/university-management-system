using Applications.DTOs.Users;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IUserService
{
    Task<Result<UserResponse>> AddAsync(CreateUserRequest request);
    Task<Result> ChangePasswordAsync(int id, ChangePasswordRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> DeleteAsync(string username);
    Task<Result<UserResponse>> GetByIdAsync(int id);
    Task<Result<UserResponse>> GetByUsernameAsync(string username);
    Task<Result<IReadOnlyCollection<UserResponse>>> GetListAsync();
    Task<Result> UpdateAsync(int id, UpdateUserRequest request);
    Task<Result<IReadOnlyCollection<UserResponse>>> GetByRoleAsync(int roleId);
}