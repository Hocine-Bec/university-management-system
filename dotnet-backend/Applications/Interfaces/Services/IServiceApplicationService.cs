using Applications.DTOs.ServiceApplication;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IServiceApplicationService
{
    Task<Result<IReadOnlyCollection<ServiceApplicationResponse>>> GetListAsync();
    Task<Result<ServiceApplicationResponse>> GetByIdAsync(int id);
    Task<Result<ServiceApplicationResponse>> GetByPersonIdAsync(int personId);
    Task<Result<ServiceApplicationResponse>> AddAsync(ServiceApplicationCreateRequest request);
    Task<Result> UpdateAsync(int id, ServiceApplicationUpdateRequest request);
    Task<Result> UpdateStatusAsync(int id, ServiceApplicationUpdateStatusRequest request);
    Task<Result> DeleteAsync(int id);
}