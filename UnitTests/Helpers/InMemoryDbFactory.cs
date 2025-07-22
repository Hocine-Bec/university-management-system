using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace UnitTests.Helpers;

public static class InMemoryDbFactory
{
    public static async Task<AppDbContext> CreateInMemoryDbContextAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
        List<T1>? entities1 = null, List<T2>? entities2 = null, List<T3>? entities3 = null, 
        List<T4>? entities4 = null, List<T5>? entities5 = null, List<T6>? entities6 = null,
        List<T7>? entities7 = null, List<T8>? entities8 = null)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
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
        
        if (entities5?.Any() == true)
            await dbContext.Set<T5>().AddRangeAsync(entities5);
        
        if (entities6?.Any() == true)
            await dbContext.Set<T6>().AddRangeAsync(entities6);
        
        if (entities7?.Any() == true)
            await dbContext.Set<T7>().AddRangeAsync(entities7);
        
        if (entities8?.Any() == true)
            await dbContext.Set<T8>().AddRangeAsync(entities8);

        await dbContext.SaveChangesAsync();
        return dbContext;
    }

    public static Task<AppDbContext> CreateAsync()
    {
        return CreateInMemoryDbContextAsync<object, object, object, object, object, object, object, object>();
    }

    public static Task<AppDbContext> CreateAsync<T1>(List<T1> entities1) where T1 : class
    {
        return CreateInMemoryDbContextAsync<T1, object, object, object, object, object, object, object>(entities1);
    }

    public static Task<AppDbContext> CreateAsync<T1, T2>(List<T1> entities1, List<T2> entities2)
        where T1 : class
        where T2 : class
    {
        return CreateInMemoryDbContextAsync<T1, T2, object, object, object, object, object, object>(
            entities1, entities2);
    }

    public static Task<AppDbContext> CreateAsync<T1, T2, T3>(List<T1> entities1, List<T2> entities2, 
        List<T3> entities3)
        where T1 : class
        where T2 : class
        where T3 : class
    {
        return CreateInMemoryDbContextAsync<T1, T2, T3, object, object, object, object, object>(entities1, 
            entities2, entities3);
    }


    public static Task<AppDbContext> CreateAsync<T1, T2, T3, T4>(List<T1> entities1, List<T2> entities2,
        List<T3> entities3, List<T4> entities4)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
    {
        return CreateInMemoryDbContextAsync<T1, T2, T3, T4, object, object, object, object>(entities1, 
            entities2, entities3, entities4);
    }

    public static Task<AppDbContext> CreateAsync<T1, T2, T3, T4, T5>(List<T1> entities1, List<T2> entities2, 
        List<T3> entities3, List<T4> entities4, List<T5> entities5)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
    {
        return CreateInMemoryDbContextAsync<T1, T2, T3, T4, T5, object, object, object>(entities1, 
            entities2, entities3, entities4, entities5);
    }

    public static Task<AppDbContext> CreateAsync<T1, T2, T3, T4, T5, T6>(List<T1> entities1, List<T2> entities2,
        List<T3> entities3, List<T4> entities4, List<T5> entities5, List<T6> entities6)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
    {
        return CreateInMemoryDbContextAsync<T1, T2, T3, T4, T5, T6, object, object>(entities1, entities2, entities3, entities4,
            entities5, entities6);
    }
    
    public static Task<AppDbContext> CreateAsync<T1, T2, T3, T4, T5, T6, T7>(
        List<T1> entities1, List<T2> entities2, List<T3> entities3, List<T4> entities4,
        List<T5> entities5, List<T6> entities6, List<T7> entities7)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
    {
        return CreateInMemoryDbContextAsync<T1, T2, T3, T4, T5, T6, T7, object>(entities1, entities2, entities3, entities4,
            entities5, entities6, entities7);
    }
    
    public static Task<AppDbContext> CreateAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
        List<T1> entities1, List<T2> entities2, List<T3> entities3, List<T4> entities4,
        List<T5> entities5, List<T6> entities6, List<T7> entities7, List<T8> entities8)
        where T1 : class
        where T2 : class
        where T3 : class
        where T4 : class
        where T5 : class
        where T6 : class
        where T7 : class
        where T8 : class
    {
        return CreateInMemoryDbContextAsync(entities1, entities2, entities3, 
            entities4, entities5, entities6, entities7, entities8);
    }
}