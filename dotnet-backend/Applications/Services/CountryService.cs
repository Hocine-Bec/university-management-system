using Applications.DTOs;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Enums;

namespace Applications.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMyLogger _logger;

        public CountryService(ICountryRepository repository, IMapper mapper, IMyLogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Result<IReadOnlyCollection<CountryResponse>>> GetListAsync()
        {
            try
            {
                var countries = await _repository.GetListAsync();
                if (!countries.Any())
                {
                    return Result<IReadOnlyCollection<CountryResponse>>.Failure(
                        "No Countries found in the system", ErrorType.NotFound);
                }

                var countriesDto = _mapper.Map<IReadOnlyCollection<CountryResponse>>(countries);
                return Result<IReadOnlyCollection<CountryResponse>>.Success(countriesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving all countries", ex);

                return Result<IReadOnlyCollection<CountryResponse>>
                    .Failure("Failed to retrieve countries due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result<CountryResponse>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return Result<CountryResponse>.Failure("Invalid country ID provided", ErrorType.BadRequest);

            try
            {
                var country = await _repository.GetByIdAsync(id);
                if (country == null)
                    return Result<CountryResponse>.Failure("Student not found with the specified ID", ErrorType.NotFound);

                var countryDto = _mapper.Map<CountryResponse>(country);
                return Result<CountryResponse>.Success(countryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving country", ex, new { id });
                return Result<CountryResponse>.Failure(
                    "Failed to retrieve country due to a system error",
                    ErrorType.InternalServerError);
            }
        }

        public async Task<Result<CountryResponse>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Result<CountryResponse>.Failure("Country name is required", ErrorType.BadRequest);

            try
            {
                var country = await _repository.GetByNameAsync(name);

                if (country == null)
                {
                    return Result<CountryResponse>.Failure(
                        "Country not found with the specified name", ErrorType.NotFound);
                }

                var countryDto = _mapper.Map<CountryResponse>(country);
                return Result<CountryResponse>.Success(countryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving country", ex, new { name });
                return Result<CountryResponse>.Failure("Failed to retrieve country due to a system error", ErrorType.InternalServerError);
            }
        }

        public async Task<Result<CountryResponse>> GetByCodeAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
                return Result<CountryResponse>.Failure("Country code is required", ErrorType.BadRequest);

            try
            {
                var country = await _repository.GetByCodeAsync(code);

                if (country == null)
                {
                    return Result<CountryResponse>.Failure(
                        "Country not found with the specified code", ErrorType.NotFound);
                }

                var countryDto = _mapper.Map<CountryResponse>(country);
                return Result<CountryResponse>.Success(countryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Database error retrieving country", ex, new { code });
                return Result<CountryResponse>.Failure("Failed to retrieve country due to a system error",
                    ErrorType.InternalServerError);
            }
        }
    }
}
