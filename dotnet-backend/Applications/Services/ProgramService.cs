using Applications.DTOs.Program;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class ProgramService : IProgramService
{
    private readonly IProgramRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;

    public ProgramService(IProgramRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<ProgramResponse>>> GetListAsync()
    {
        try
        {
            var programs = await _repository.GetListAsync();
            if (!programs.Any())
            {
                return Result<IReadOnlyCollection<ProgramResponse>>.Failure("No programs found in the system", ErrorType.NotFound);
            }

            var response = _mapper.Map<IReadOnlyCollection<ProgramResponse>>(programs);
            return Result<IReadOnlyCollection<ProgramResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving all programs", ex);
            return Result<IReadOnlyCollection<ProgramResponse>>
                .Failure("Failed to retrieve programs due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<ProgramResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<ProgramResponse>.Failure("Invalid program ID provided", ErrorType.BadRequest);

        try
        {
            var program = await _repository.GetByIdAsync(id);
            if (program == null)
                return Result<ProgramResponse>.Failure("Program not found with the specified ID", ErrorType.NotFound);

            var response = _mapper.Map<ProgramResponse>(program);
            return Result<ProgramResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving program", ex, new { id });
            return Result<ProgramResponse>.Failure("Failed to retrieve program due to a system error",
                ErrorType.InternalServerError);
        }
    }

    public async Task<Result<ProgramResponse>> GetByCodeAsync(string code)
    {
        if (string.IsNullOrEmpty(code))
            return Result<ProgramResponse>.Failure("Program code is required", ErrorType.BadRequest);

        try
        {
            var program = await _repository.GetByCodeAsync(code);
            if (program == null)
                return Result<ProgramResponse>.Failure("Program not found with the specified code", ErrorType.NotFound);

            var response = _mapper.Map<ProgramResponse>(program);
            return Result<ProgramResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving program", ex, new { code });
            return Result<ProgramResponse>.Failure("Failed to retrieve program due to a system error",
                ErrorType.InternalServerError);
        }
    }

    public async Task<Result<ProgramResponse>> AddAsync(ProgramRequest request)
    {
        if (request == default)
            return Result<ProgramResponse>.Failure("Program information is required", ErrorType.BadRequest);
        
        try
        {
            var isExist = await _repository.DoesExistAsync(request.Code ?? string.Empty);
            if (isExist)
                return Result<ProgramResponse>.Failure("Program with this code already exists", ErrorType.Conflict);

            var program = _mapper.Map<Program>(request);
            
            int id = await _repository.AddAsync(program);
            if (id <= 0)
                return Result<ProgramResponse>.Failure("Failed to create new program record", ErrorType.BadRequest);

            var response = _mapper.Map<ProgramResponse>(program);
            return Result<ProgramResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error adding new program", ex, new { request });
            return Result<ProgramResponse>.Failure("Failed to create program due to a system error",
                ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, ProgramRequest request)
    {
        if (request == default)
            return Result.Failure("Program information is required for update", ErrorType.BadRequest);
        
        try
        {
            var program = await _repository.GetByIdAsync(id);
            if (program == null)
                return Result.Failure("Program Not Found", ErrorType.NotFound);

            _mapper.Map(request, program);
            program.Id = id;
            
            bool isUpdated = await _repository.UpdateAsync(program);
            return !isUpdated ? Result.Failure($"Failed to update program", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating program", ex, new { request });
            return Result.Failure("Failed to update program due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid program ID provided", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Program not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting program", ex, new { id });
            return Result.Failure("Failed to delete program due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(string code)
    {
        if (string.IsNullOrEmpty(code))
            return Result.Failure("Program code is required", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(code);
            return !isDeleted ? Result.Failure("Program not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting program", ex, new { code });
            return Result.Failure("Failed to delete program due to a system error", ErrorType.InternalServerError);
        }
    }
}