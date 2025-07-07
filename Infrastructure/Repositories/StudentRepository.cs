using Applications.Interfaces.Repositories;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context) { }

        public async Task<bool> DeleteAsync(string studentNumber)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);

            if (student == null)
                return false;

            _context.Students.Remove(student);
            return await _context.SaveChangesAsync() > 0;
        }

        public override async Task<IReadOnlyCollection<Student>> GetListAsync()
        {
            return await _context.Set<Student>()
                .AsNoTracking()
                .Include(p => p.Person)
                .ToListAsync();
        }

        public async Task<bool> DoesExistAsync(int personId)
        {
            return await _context.Students.AnyAsync(x => x.PersonId == personId);
        }

        public async Task<bool> DoesExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(x => x.Id == id);
        }

        public override async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .AsNoTracking()
                .Include(p => p.Person)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Student?> GetByStudentNumberAsync(string studentNumber)
        {
            return await _context.Students
                .AsNoTracking()
                .Include(p => p.Person)
                .FirstOrDefaultAsync(n => n.StudentNumber == studentNumber);
        }
    }
}
