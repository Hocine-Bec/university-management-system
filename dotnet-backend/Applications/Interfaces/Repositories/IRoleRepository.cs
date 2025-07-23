using Applications.Interfaces.Base;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Interfaces.Repositories;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<Role?> GetByNameAsync(SystemRole name);
}