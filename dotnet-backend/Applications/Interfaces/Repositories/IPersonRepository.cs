using Applications.Interfaces.Base;
using Applications.Shared;
using Domain.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        Task<Person?> GetByNameAsync(string lastName);
        Task<bool> DeleteAsync(string lastName);
        Task<bool> DoesExistAsync(string lastName);
    }
}
