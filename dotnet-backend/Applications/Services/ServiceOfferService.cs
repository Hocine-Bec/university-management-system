using Applications.DTOs.ServiceOffer;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class ServiceOfferService : IServiceOfferService
{
    private readonly IServiceOfferRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;
    
    public ServiceOfferService(IServiceOfferRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<Result<IReadOnlyCollection<ServiceOfferResponse>>> GetListAsync()
    {
        try
        {
            var offers = await _repository.GetListAsync();
            if (!offers.Any())
            {
                return Result<IReadOnlyCollection<ServiceOfferResponse>>.Failure(
                    "No service offers found", ErrorType.NotFound);
            }

            var response = _mapper.Map<IReadOnlyCollection<ServiceOfferResponse>>(offers);
            return Result<IReadOnlyCollection<ServiceOfferResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving all service offers", ex);
            return Result<IReadOnlyCollection<ServiceOfferResponse>>
                .Failure("Failed to retrieve service offers due to a system error", ErrorType.InternalServerError);
        }
    }
    
    public async Task<Result<ServiceOfferResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<ServiceOfferResponse>.Failure("Invalid service offer ID provided", ErrorType.BadRequest);

        try
        {
            var offer = await _repository.GetByIdAsync(id);
            if (offer == null)
                return Result<ServiceOfferResponse>.Failure("Service offer not found with the specified ID", ErrorType.NotFound);

            var response = _mapper.Map<ServiceOfferResponse>(offer);
            return Result<ServiceOfferResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving service offer", ex, new { id });
            return Result<ServiceOfferResponse>.Failure("Failed to retrieve service offer due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<ServiceOfferResponse>> AddAsync(ServiceOfferRequest request)
    {
        if (request == default)
            return Result<ServiceOfferResponse>.Failure("Service offer information is required", ErrorType.BadRequest);
        
        try
        {
            var isExist = await _repository.DoesExistAsync(request.Name ?? string.Empty);
            if (isExist)
                return Result<ServiceOfferResponse>.Failure("Service offer already exists", ErrorType.Conflict);

            var offer = _mapper.Map<ServiceOffer>(request);
            
            int id = await _repository.AddAsync(offer);
            if (id <= 0)
                return Result<ServiceOfferResponse>.Failure("Failed to create new service offer", ErrorType.BadRequest);

            var response = _mapper.Map<ServiceOfferResponse>(offer);
            return Result<ServiceOfferResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error adding new service offer", ex, new { request });
            return Result<ServiceOfferResponse>.Failure("Failed to create service offer due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, ServiceOfferRequest request)
    {
        if (request == default)
            return Result.Failure("Service offer information is required for update", ErrorType.BadRequest);
        
        try
        {
            var service = await _repository.GetByIdAsync(id);
            if (service == null)
                return Result.Failure("Service offer not found", ErrorType.NotFound);

            _mapper.Map(request, service);
            
            bool isUpdated = await _repository.UpdateAsync(service);
            return !isUpdated ? Result.Failure("Failed to update service offer", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating service offer", ex, new { request });
            return Result.Failure("Failed to update service offer due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid service offer ID provided", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Service offer not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting service offer", ex, new { id });
            return Result.Failure("Failed to delete service offer due to a system error", ErrorType.InternalServerError);
        }
    }
}