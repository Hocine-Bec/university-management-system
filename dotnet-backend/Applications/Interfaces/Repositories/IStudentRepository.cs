using Applications.Interfaces.Base;
using Domain.Entities;

namespace Applications.Interfaces.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<Student?> GetByStudentNumberAsync(string studentNumber);
        Task<bool> DeleteAsync(string studentNumber);
        Task<bool> DoesExistAsync(int personId);
        Task<bool> DoesExistsAsync(int studentId);
    }
}
