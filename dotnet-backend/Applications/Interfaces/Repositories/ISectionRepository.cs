using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface ISectionRepository : IGenericRepository<Section>
{
    Task<bool> DeleteAsync(string sectionNumber);
    Task<bool> DoesExistAsync(string sectionNumber);
    Task<Section?> GetBySectionNumberAsync(string sectionNumber);
}