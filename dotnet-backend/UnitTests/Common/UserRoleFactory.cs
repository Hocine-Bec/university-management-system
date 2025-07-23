using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class UserRoleFactory
{
    public static List<UserRole> CreateTestUserRoles(int count, List<User> users, List<Role> roles, 
        int? seed = null)
    {
        if (count > users.Count || count > roles.Count)
            throw new ArgumentException("Cannot create more user roles than available users or roles");

        var faker = new Faker<UserRole>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        var shuffledUsers = users.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var shuffledRoles = roles.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var index = 0;

        faker.RuleFor(ur => ur.Id, f => f.IndexFaker + 1)
            .RuleFor(ur => ur.IsActive, f => f.Random.Bool(0.8f))
            .FinishWith((f, ur) =>
            {
                ur.UserId = shuffledUsers[index].Id;
                ur.User = shuffledUsers[index];
                ur.RoleId = shuffledRoles[index].Id;
                ur.Role = shuffledRoles[index];
                index++;
            });

        return faker.Generate(count);
    }
}