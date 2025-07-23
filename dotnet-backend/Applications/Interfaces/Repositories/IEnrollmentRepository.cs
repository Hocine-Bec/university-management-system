using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories;

public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    Task<Enrollment?> GetByStudentIdAsync(int studentId);
    Task<bool> CanEnrollInProgramAsync(int studentId, int programId);
}