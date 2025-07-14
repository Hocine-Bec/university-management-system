using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace UnitTests.Helpers;

public static class InMemoryDbFactory
{
    public static async Task<AppDbContext> CreateInMemoryDbContext(List<Country> countries = null!)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);
        await dbContext.Countries.AddRangeAsync(countries);
        await dbContext.SaveChangesAsync();

        return dbContext;
    }
}