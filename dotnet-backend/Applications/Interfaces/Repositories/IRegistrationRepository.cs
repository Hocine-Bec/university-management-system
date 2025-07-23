using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface IRegistrationRepository : IGenericRepository<Registration>
{
    Task<bool> DoesExistAsync(int studentId, int sectionId, int semesterId);
}