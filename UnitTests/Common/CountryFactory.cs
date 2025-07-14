using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class CountryFactory
{
    public static List<Country> CreateTestCountries(int count = 5, int? seed = null)
    {
        var faker = new Faker<Country>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.Name, f => f.Address.Country())
            .RuleFor(c => c.Code, f => f.Address.CountryCode());
        
        if (seed.HasValue)
            faker.UseSeed(seed.Value);
        
        return faker.Generate(count);
    }
}