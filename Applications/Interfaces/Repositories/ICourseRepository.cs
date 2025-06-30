using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<Course?> GetByCodeAsync(string code);
    Task<bool> DoesCodeExistAsync(string code);
}