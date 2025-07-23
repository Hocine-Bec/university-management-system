using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.Common;

public static class RoleFactory
{
    public static List<Role> CreateTestRoles(int count, int? seed = null)
    {
        var faker = new Faker<Role>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        faker.RuleFor(r => r.Id, f => f.IndexFaker + 1)
            .RuleFor(r => r.Name, f => f.PickRandom<SystemRole>())
            .RuleFor(r => r.Description, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }
}