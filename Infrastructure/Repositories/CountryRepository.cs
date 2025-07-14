using Applications.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly AppDbContext _context;

    public CountryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Country>> GetListAsync()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country?> GetByIdAsync(int id)
    {
        return await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Country?> GetByCodeAsync(string code)
    {
        return await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<Country?> GetByNameAsync(string name)
    {
        return await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name);
    }
}