using Applications.DTOs.People;
using Applications.Shared;

namespace Applications.Interfaces.Services
{
    public interface IPersonService
    {
        Task<Result<PersonResponse>> AddAsync(PersonRequest request);
        Task<Result> DeleteAsync(int id);
        Task<Result> DeleteAsync(string lastName);
        Task<Result<PersonResponse>> GetByIdAsync(int id);
        Task<Result<PersonResponse>> GetByNameAsync(string lastName);
        Task<Result<IReadOnlyCollection<PersonResponse>>> GetListAsync();
        Task<Result> UpdateAsync(int id, PersonRequest request);
    }
}
