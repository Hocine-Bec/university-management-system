using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface IDocsVerificationRepository : IGenericRepository<DocsVerification>
{
    Task<DocsVerification?> GetByPersonIdAsync(int personId);
}