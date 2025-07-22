using Bogus;
using Domain.Entities;
using Person = Domain.Entities.Person;

namespace UnitTests.Common;

public static class UserFactory
{
    public static List<User> CreateTestUsers(int count, List<Person> people, int? seed = null)
    {
        if (count > people.Count)
            throw new ArgumentException("Cannot create more users than available people");

        var faker = new Faker<User>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        var shuffledPeople = people.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var personIndex = 0;

        faker.RuleFor(u => u.Id, f => f.IndexFaker + 1)
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.IsActive, f => f.Random.Bool(0.8f))
            .RuleFor(u => u.CreatedAt, f => f.Date.Past(2))
            .RuleFor(u => u.LastLoginAt, f => f.Random.Bool(0.7f) ? f.Date.Recent() : null)
            .FinishWith((f, u) =>
            {
                u.PersonId = shuffledPeople[personIndex].Id;
                u.Person = shuffledPeople[personIndex];
                personIndex++;
            });

        return faker.Generate(count);
    }
}