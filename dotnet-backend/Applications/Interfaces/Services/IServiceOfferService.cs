using Applications.DTOs.ServiceOffer;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IServiceOfferService
{
    Task<Result<IReadOnlyCollection<ServiceOfferResponse>>> GetListAsync();
    Task<Result<ServiceOfferResponse>> GetByIdAsync(int id);
    Task<Result<ServiceOfferResponse>> AddAsync(ServiceOfferRequest request);
    Task<Result> UpdateAsync(int id, ServiceOfferRequest request);
    Task<Result> DeleteAsync(int id);
}