using Applications.DTOs.Professor;
using Applications.Helpers;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class ProfessorService : IProfessorService
{
    private readonly IProfessorRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;

    public ProfessorService(IProfessorRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<ProfessorResponse>>> GetListAsync()
    {
        try
        {
            var professors = await _repository.GetListAsync();
            if (!professors.Any())
            {
                return Result<IReadOnlyCollection<ProfessorResponse>>.Failure(
                    "No professors found in the system", ErrorType.NotFound);
            }

            var response = _mapper.Map<IReadOnlyCollection<ProfessorResponse>>(professors);
            return Result<IReadOnlyCollection<ProfessorResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving all professors", ex);
            return Result<IReadOnlyCollection<ProfessorResponse>>
                .Failure("Failed to retrieve professors due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<ProfessorResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<ProfessorResponse>.Failure("Invalid professor ID provided", ErrorType.BadRequest);

        try
        {
            var professor = await _repository.GetByIdAsync(id);
            if (professor == null)
                return Result<ProfessorResponse>.Failure("Professor not found with the specified ID",
                    ErrorType.NotFound);

            var response = _mapper.Map<ProfessorResponse>(professor);
            return Result<ProfessorResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving professor", ex, new { id });
            return Result<ProfessorResponse>.Failure("Failed to retrieve professor due to a system error",
                ErrorType.InternalServerError);
        }
    }

    public async Task<Result<ProfessorResponse>> GetByEmployeeNumberAsync(string employeeNumber)
    {
        if (string.IsNullOrEmpty(employeeNumber))
            return Result<ProfessorResponse>.Failure("Employee number is required", ErrorType.BadRequest);

        try
        {
            var professor = await _repository.GetByEmployeeNumberAsync(employeeNumber);
            if (professor == null)
            {
                return Result<ProfessorResponse>.Failure(
                    "Professor not found with the specified employee number", ErrorType.NotFound);
            }

            var response = _mapper.Map<ProfessorResponse>(professor);
            return Result<ProfessorResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving professor", ex, new { employeeNumber });
            return Result<ProfessorResponse>.Failure("Failed to retrieve professor due to a system error",
                ErrorType.InternalServerError);
        }
    }

    public async Task<Result<ProfessorResponse>> AddAsync(ProfessorRequest request)
    {
        if (request == default)
            return Result<ProfessorResponse>.Failure("Professor information is required", ErrorType.BadRequest);
        
        try
        {
            var isExist = await _repository.DoesExistAsync(request.PersonId);
            if (isExist)
                return Result<ProfessorResponse>.Failure("Professor already exists", ErrorType.Conflict);

            var professor = _mapper.Map<Professor>(request);
            professor.EmployeeNumber = professor.GenerateUniqueNumber();
            
            int id = await _repository.AddAsync(professor);
            if (id <= 0)
                return Result<ProfessorResponse>.Failure("Failed to create new professor record", ErrorType.BadRequest);

            var response = _mapper.Map<ProfessorResponse>(professor);
            return Result<ProfessorResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error adding new professor", ex, new { request });
            return Result<ProfessorResponse>.Failure("Failed to create professor due to a system error",
                ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, ProfessorRequest request)
    {
        if (request == default)
            return Result.Failure("Professor information is required for update", ErrorType.BadRequest);
        
        try
        {
            var professor = await _repository.GetByIdAsync(id);
            if (professor == null)
                return Result.Failure("Professor Not Found", ErrorType.NotFound);

            _mapper.Map(request, professor);
            professor.Id = id;
            
            bool isUpdated = await _repository.UpdateAsync(professor);
            return !isUpdated ? Result.Failure($"Failed to update professor", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating professor", ex, new { request });
            return Result.Failure("Failed to update professor due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid professor ID provided", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Professor not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting professor", ex, new { id });
            return Result.Failure("Failed to delete professor due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(string employeeNumber)
    {
        if (string.IsNullOrEmpty(employeeNumber))
            return Result.Failure("Employee number is required", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(employeeNumber);
            return !isDeleted ? Result.Failure("Professor not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting professor", ex, new { employeeNumber });
            return Result.Failure("Failed to delete professor due to a system error", ErrorType.InternalServerError);
        }
    }
}