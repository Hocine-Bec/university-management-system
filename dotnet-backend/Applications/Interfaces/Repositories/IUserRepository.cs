using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> DeleteAsync(string username);
        Task<bool> DoesExistAsync(int personId);
        Task<IReadOnlyCollection<User>> GetByRoleAsync(int roleId);
    }
}
