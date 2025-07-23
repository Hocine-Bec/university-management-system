using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface IFinancialHoldRepository : IGenericRepository<FinancialHold>
{
    Task<IReadOnlyCollection<FinancialHold>> GetByStudentIdAsync(int studentId);
}