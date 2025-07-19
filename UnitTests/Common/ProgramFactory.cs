using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class ProgramFactory
{
    public static List<Program> CreateTestPrograms(int count = 5, int? seed = null)
    {
        var faker = new Faker<Program>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        faker.RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Code, f => f.Random.AlphaNumeric(5).ToUpper())
            .RuleFor(p => p.Name, f => f.Commerce.Department())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.MinimumAge, f => f.Random.Int(18, 25))
            .RuleFor(p => p.Duration, f => f.Random.Int(2, 5))
            .RuleFor(p => p.Fees, f => f.Finance.Amount(1000, 5000))
            .RuleFor(p => p.IsActive, f => f.Random.Bool());

        return faker.Generate(count);
    }
}
