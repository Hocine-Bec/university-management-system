using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProfessorRepository(AppDbContext context) : GenericRepository<Professor>(context), IProfessorRepository
{
    //Entity related methods
    public async Task<bool> DoesExistAsync(int personId)
    {
        return await _context.Professors.AnyAsync(x => x.PersonId == personId);
    }
    public override async Task<Professor?> GetByIdAsync(int id)
    {
        return await _context.Professors
            .AsNoTracking()
            .Include(p => p.Person)
            .FirstOrDefaultAsync(n => n.Id == id);
    }
    
    public async Task<Professor?> GetByEmployeeNumberAsync(string employeeNumber)
    {
        return await _context.Professors
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.EmployeeNumber == employeeNumber);
    }
    
    public async Task<bool> DeleteAsync(string employeeNumber)
    {
        var professor = await _context.Professors.FirstOrDefaultAsync(s => s.EmployeeNumber == employeeNumber);

        if (professor == null)
            return false;

        _context.Professors.Remove(professor);
        return await _context.SaveChangesAsync() > 0;
    }
}