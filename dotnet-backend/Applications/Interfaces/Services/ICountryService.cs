using Applications.DTOs;
using Applications.Shared;

namespace Applications.Interfaces.Services
{
    public interface ICountryService
    {
        Task<Result<CountryResponse>> GetByCodeAsync(string code);
        Task<Result<CountryResponse>> GetByIdAsync(int id);
        Task<Result<CountryResponse>> GetByNameAsync(string name);
        Task<Result<IReadOnlyCollection<CountryResponse>>> GetListAsync();
    }
}