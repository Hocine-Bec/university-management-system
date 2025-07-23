using Applications.Interfaces.Base;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Interfaces.Repositories;

public interface IServiceApplicationRepository : IGenericRepository<ServiceApplication>
{
    Task<bool> DoesPersonHaveActiveApplicationsAsync(int personId, int serviceOfferId);
    Task<ServiceApplication?> GetByPersonIdAsync(int personId);
    Task<bool> DoesExistsAsync(int id);
}