using Applications.DTOs;
using Applications.Shared;
using Domain.Enums;

namespace Applications.Interfaces.Services;

public interface IRoleService
{
    Task<Result<RoleDto>> GetByIdAsync(int id);
    Task<Result<RoleDto>> GetByRoleAsync(int roleType);
    Task<Result<IReadOnlyCollection<RoleDto>>> GetListAsync();
    Task<Result<RoleDto>> AddAsync(RoleDto request);
    Task<Result> UpdateAsync(int id, RoleDto request);
    Task<Result> DeleteAsync(int id);
}