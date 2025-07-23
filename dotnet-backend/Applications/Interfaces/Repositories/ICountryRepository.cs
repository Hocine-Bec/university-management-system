using Domain.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface ICountryRepository
    {
        Task<Country?> GetByCodeAsync(string code);
        Task<Country?> GetByIdAsync(int id);
        Task<Country?> GetByNameAsync(string name);
        Task<IEnumerable<Country>> GetListAsync();
    }
}
