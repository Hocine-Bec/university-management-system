using Applications.DTOs.Section;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class SectionService : ISectionService
{
    private readonly ISectionRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;

    public SectionService(ISectionRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<IReadOnlyCollection<SectionResponse>>> GetListAsync()
    {
        try
        {
            var sections = await _repository.GetListAsync();
            if (!sections.Any())
            {
                return Result<IReadOnlyCollection<SectionResponse>>.Failure(
                    "No sections found in the system", ErrorType.NotFound);
            }

            var response = _mapper.Map<IReadOnlyCollection<SectionResponse>>(sections);
            return Result<IReadOnlyCollection<SectionResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving all sections", ex);
            return Result<IReadOnlyCollection<SectionResponse>>
                .Failure("Failed to retrieve sections due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<SectionResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<SectionResponse>.Failure("Invalid section ID provided", ErrorType.BadRequest);

        try
        {
            var section = await _repository.GetByIdAsync(id);
            if (section == null)
                return Result<SectionResponse>.Failure("Section not found with the specified ID", ErrorType.NotFound);

            var response = _mapper.Map<SectionResponse>(section);
            return Result<SectionResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving section", ex, new { id });
            return Result<SectionResponse>.Failure("Failed to retrieve section due to a system error",
                ErrorType.InternalServerError);
        }
    }

    public async Task<Result<SectionResponse>> GetBySectionNumberAsync(string sectionNumber)
    {
        if (string.IsNullOrEmpty(sectionNumber))
            return Result<SectionResponse>.Failure("Section number is required", ErrorType.BadRequest);

        try
        {
            var section = await _repository.GetBySectionNumberAsync(sectionNumber);
            if (section == null)
                return Result<SectionResponse>.Failure(
                    "Section not found with the specified section number", ErrorType.NotFound);

            var response = _mapper.Map<SectionResponse>(section);
            return Result<SectionResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving section", ex, new { sectionNumber });
            return Result<SectionResponse>.Failure("Failed to retrieve section due to a system error",
                ErrorType.InternalServerError);
        }
    }

    public async Task<Result<SectionResponse>> AddAsync(SectionRequest request)
    {
        if (request == default)
            return Result<SectionResponse>.Failure("Section information is required", ErrorType.BadRequest);

        try
        {
            var isExist = await _repository.DoesExistAsync(request.SectionNumber ?? string.Empty);
            if (isExist)
                return Result<SectionResponse>.Failure("Section with this number already exists", ErrorType.Conflict);

            var section = _mapper.Map<Section>(request);
            
            int id = await _repository.AddAsync(section);
            if (id <= 0)
                return Result<SectionResponse>.Failure("Failed to create new section record", ErrorType.BadRequest);

            var response = _mapper.Map<SectionResponse>(section);
            return Result<SectionResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error adding new section", ex, new { request });
            return Result<SectionResponse>.Failure("Failed to create section due to a system error",
                ErrorType.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, SectionRequest request)
    {
        if (request == default)
            return Result.Failure("Section information is required for update", ErrorType.BadRequest);

        try
        {
            var section = await _repository.GetByIdAsync(id);
            if (section == null)
                return Result.Failure("Section Not Found", ErrorType.NotFound);

            _mapper.Map(request, section);
            section.Id = id;
            
            bool isUpdated = await _repository.UpdateAsync(section);
            return !isUpdated ? Result.Failure($"Failed to update section", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating section", ex, new { request });
            return Result.Failure("Failed to update section due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid section ID provided", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Section not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting section", ex, new { id });
            return Result.Failure("Failed to delete section due to a system error", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(string sectionNumber)
    {
        if (string.IsNullOrEmpty(sectionNumber))
            return Result.Failure("Section number is required", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(sectionNumber);
            return !isDeleted ? Result.Failure("Section not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting section", ex, new { sectionNumber });
            return Result.Failure("Failed to delete section due to a system error", ErrorType.InternalServerError);
        }
    }
}