using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface IPrerequisiteRepository : IGenericRepository<Prerequisite>
{
    Task<Prerequisite?> GetByCourseIdAsync(int courseId);
    Task<bool> DeleteForCourseAsync(int courseId);
    Task<bool> DoesExistsAsync(int requestCourseId, int requestPrerequisiteCourseId);
}