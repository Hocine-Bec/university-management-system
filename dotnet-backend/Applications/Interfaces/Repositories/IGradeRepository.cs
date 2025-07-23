using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface IGradeRepository : IGenericRepository<Grade>
{
    Task<IReadOnlyCollection<Grade>> GetByStudentIdAsync(int studentId);
    Task<IReadOnlyCollection<Grade>> GetByCourseIdAsync(int courseId);
    Task<bool> DoesExistAsync(int studentId, int courseId, int semesterId);
}