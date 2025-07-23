using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface IProgramRepository : IGenericRepository<Program>
{
    Task<bool> DoesExistAsync(string code);
    Task<bool> DeleteAsync(string code);
    Task<Program?> GetByCodeAsync(string code);
    Task<bool> DoesExistsAsync(int id);
}