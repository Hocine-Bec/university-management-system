using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface IProfessorRepository : IGenericRepository<Professor>
{
    Task<bool> DoesExistAsync(int personId);
    Task<bool> DeleteAsync(string employeeNumber);
    Task<Professor?> GetByEmployeeNumberAsync(string employeeNumber);
}