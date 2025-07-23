using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface ISemesterRepository : IGenericRepository<Semester>
{
    Task<Semester?> GetByTermCodeAsync(string termCode);
}