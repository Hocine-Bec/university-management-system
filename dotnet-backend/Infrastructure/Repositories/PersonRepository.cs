using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PersonRepository : GenericRepository<Person>, IPersonRepository
{
    public PersonRepository(AppDbContext context) : base(context)
    {
    }

    //Entity Related Operations
    public async Task<bool> DeleteAsync(string lastName)
    {
        var person = await _context.People.FirstOrDefaultAsync(n => n.LastName == lastName);

        if (person == null)
            return false;

        _context.People.Remove(person);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Person?> GetByNameAsync(string lastName)
    {
        return await _context.People
            .AsNoTracking()
            .Include(c => c.Country)
            .FirstOrDefaultAsync(n => n.LastName == lastName);
    }
        
    public async Task<bool> DoesExistAsync(string lastName)
    {
        return await _context.People.AnyAsync(x => x.LastName == lastName);
    }

    public override async Task<Person?> GetByIdAsync(int id)
    {
        return await _context.People
            .AsNoTracking()
            .Include(c => c.Country)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<IReadOnlyCollection<Person>> GetListAsync()
    {
        return await _context.People
            .AsNoTracking()
            .Include(c => c.Country)
            .ToListAsync();
    }
}