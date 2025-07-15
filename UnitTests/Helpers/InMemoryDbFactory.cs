using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace UnitTests.Helpers;

public static class InMemoryDbFactory
{
    public static async Task<AppDbContext> CreateInMemoryDbContextAsync<T1, T2, T3, T4>(
        List<T1>? entities1 = null,
        List<T2>? entities2 = null,
        List<T3>? entities3 = null,
        List<T4>? entities4 = null)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new AppDbContext(options);

        if (entities1?.Any() == true)
            await dbContext.Set<T1>().AddRangeAsync(entities1);

        if (entities2?.Any() == true)
            await dbContext.Set<T2>().AddRangeAsync(entities2);

        if (entities3?.Any() == true)
            await dbContext.Set<T3>().AddRangeAsync(entities3);

        if (entities4?.Any() == true)
            await dbContext.Set<T4>().AddRangeAsync(entities4);

        await dbContext.SaveChangesAsync();
        return dbContext;
    }

    public static Task<AppDbContext> CreateAsync()
        => CreateInMemoryDbContextAsync<object, object, object, object>();
    
    public static Task<AppDbContext> CreateAsync<T1>(List<T1> entities1) where T1 : class
        => CreateInMemoryDbContextAsync<T1, object, object, object>(entities1);

    public static Task<AppDbContext> CreateAsync<T1, T2>(
        List<T1> entities1, 
        List<T2> entities2)
        where T1 : class
        where T2 : class
        => CreateInMemoryDbContextAsync<T1, T2, object, object>(entities1, entities2);

    public static Task<AppDbContext> CreateAsync<T1, T2, T3>(
        List<T1> entities1, 
        List<T2> entities2, 
        List<T3> entities3)
        where T1 : class
        where T2 : class
        where T3 : class
        => CreateInMemoryDbContextAsync<T1, T2, T3, object>(entities1, entities2, entities3);


    public static Task<AppDbContext> CreateAsync<T1, T2, T3, T4>(
        List<T1> entities1, 
        List<T2> entities2,
        List<T3> entities3,
        List<T4> entities4)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        => CreateInMemoryDbContextAsync<T1, T2, T3, T4>(entities1, entities2, entities3, entities4);
}