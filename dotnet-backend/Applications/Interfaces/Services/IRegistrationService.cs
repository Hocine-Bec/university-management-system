using Applications.DTOs.Registration;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IRegistrationService
{
    Task<Result<IReadOnlyCollection<RegistrationResponse>>> GetListAsync();
    Task<Result<RegistrationResponse>> GetByIdAsync(int id);
    Task<Result<RegistrationResponse>> AddAsync(RegistrationRequest request);
    Task<Result> UpdateAsync(int id, RegistrationRequest request);
    Task<Result> DeleteAsync(int id);
}