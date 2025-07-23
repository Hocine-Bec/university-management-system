using Applications.DTOs.FinancialHold;
using Applications.Helpers;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Services;
using Applications.Interfaces.UnitOfWorks;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class FinancialHoldService : IFinancialHoldService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger; 

    public FinancialHoldService(IUnitOfWork uow, IMapper mapper, IMyLogger logger)
    {
        _uow = uow;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<FinancialHoldResponse>>> GetListAsync()
    {
        try
        {
            var holds = await _uow.FinancialHolds.GetListAsync();
            if(!holds.Any())
                return Result<IReadOnlyCollection<FinancialHoldResponse>>.Failure("No holds were found", ErrorType.NotFound);
            
            var response = _mapper.Map<IReadOnlyCollection<FinancialHoldResponse>>(holds);
            return Result<IReadOnlyCollection<FinancialHoldResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving all financial holds", ex);
            return Result<IReadOnlyCollection<FinancialHoldResponse>>.Failure(
                "Failed to retrieve financial holds", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<FinancialHoldResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<FinancialHoldResponse>.Failure("Invalid Financial Hold  ID", ErrorType.BadRequest);

        try
        {
            var hold = await _uow.FinancialHolds.GetByIdAsync(id);
            if (hold == null)
                return Result<FinancialHoldResponse>.Failure("Financial hold not found", ErrorType.NotFound);

            var response =  _mapper.Map<FinancialHoldResponse>(hold);
            return Result<FinancialHoldResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving financial hold", ex, new { id });
            return Result<FinancialHoldResponse>.Failure(
                "Failed to retrieve financial hold", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<IReadOnlyCollection<FinancialHoldResponse>>> GetByStudentIdAsync(int studentId)
    {
        if (studentId <= 0)
            return Result<IReadOnlyCollection<FinancialHoldResponse>>.Failure("Invalid student ID", ErrorType.BadRequest);

        try
        {
            var holds = await _uow.FinancialHolds.GetByStudentIdAsync(studentId);
            if(!holds.Any())
                return Result<IReadOnlyCollection<FinancialHoldResponse>>.Failure("No holds were found", ErrorType.NotFound);
            
            var response = _mapper.Map<IReadOnlyCollection<FinancialHoldResponse>>(holds);
            return Result<IReadOnlyCollection<FinancialHoldResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving financial holds by student ID", ex, new { studentId });
            return Result<IReadOnlyCollection<FinancialHoldResponse>>.Failure(
                "Failed to retrieve student holds", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<FinancialHoldResponse>> AddAsync(FinancialHoldRequest request)
    {
        try
        {
            var result = await request.ValidateFinancialHoldRequestAsync(_uow);
            if (!result.IsSuccess)
                return Result<FinancialHoldResponse>.Failure(result.Error, result.ErrorType);
            
            var hold = _mapper.Map<FinancialHold>(request);
            hold.DatePlaced = DateTime.UtcNow;
            hold.IsActive = true;

            var id = await _uow.FinancialHolds.AddAsync(hold);
            if(id <= 0)
                return Result<FinancialHoldResponse>.Failure("Failed to add a financial hold", ErrorType.BadRequest);
            
            var response = _mapper.Map<FinancialHoldResponse>(hold);
            return Result<FinancialHoldResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error adding financial hold", ex, new { request });
            return Result<FinancialHoldResponse>.Failure("Failed to create financial hold",
                ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, FinancialHoldRequest request)
    {
        try
        {
            var result = await request.ValidateFinancialHoldRequestAsync(_uow);
            if (!result.IsSuccess)
                return Result<FinancialHoldResponse>.Failure(result.Error, result.ErrorType);

            var existingHold = await _uow.FinancialHolds.GetByIdAsync(id);
            if (existingHold == null)
                return Result.Failure("Hold not found", ErrorType.NotFound);

            _mapper.Map(request, existingHold);
            existingHold.Id = id;
            
            bool isUpdated = await _uow.FinancialHolds.UpdateAsync(existingHold);
            return !isUpdated ? Result.Failure("Failed to update the financial hold", ErrorType.BadRequest) 
                : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating financial hold", ex, new { id, request });
            return Result.Failure("Failed to update financial hold", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> ResolveHoldAsync(int id, ResolveRequest request)
    {
        if(request == default)
            return Result.Failure("Resolution data is required", ErrorType.BadRequest);
        
        try
        {
            var hold = await _uow.FinancialHolds.GetByIdAsync(id);
            if (hold == null)
                return Result.Failure("Hold not found", ErrorType.NotFound);

            if (string.IsNullOrWhiteSpace(request.ResolutionNotes))
                return Result.Failure("Resolution notes are required", ErrorType.BadRequest);
                    
            if (request.ResolvedByUserId <= 0)
                return Result.Failure("A valid user ID is required to resolve the hold.", ErrorType.BadRequest);
            
            hold.DateResolved = DateTime.UtcNow;
            hold.ResolutionNotes = request.ResolutionNotes;
            hold.ResolvedByUserId = request.ResolvedByUserId;
            hold.IsActive = false;
            
            bool isUpdated = await _uow.FinancialHolds.UpdateAsync(hold);
            return !isUpdated ? Result.Failure("Failed to resolve the financial hold", ErrorType.BadRequest) 
                : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error resolving financial hold", ex, new { id, request });
            return Result.Failure("Failed to resolve hold", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result<FinancialHoldResponse>.Failure("Invalid Financial Hold ID", ErrorType.BadRequest);
        
        try
        {
            var isDeleted = await _uow.FinancialHolds.DeleteAsync(id);
            return isDeleted ? Result.Success : Result.Failure("Hold not found", ErrorType.NotFound);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting financial hold", ex, new { id });
            return Result.Failure("Failed to delete financial hold", ErrorType.InternalServerError);
        }
    }
}