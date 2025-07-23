using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class FinancialHoldFactory
{
    public static List<FinancialHold> CreateTestFinancialHolds(
        int count,
        List<Student> students,
        List<User> users,
        int? seed = null)
    {
        if (count > students.Count || count > users.Count)
            throw new ArgumentException("Cannot create more holds than available students or users");

        var faker = new Faker<FinancialHold>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        var shuffledStudents = students.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var shuffledUsers = users.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var index = 0;

        faker.RuleFor(f => f.Id, f => f.IndexFaker + 1)
            .RuleFor(f => f.Reason, f => f.Lorem.Sentence())
            .RuleFor(f => f.HoldAmount, f => f.Finance.Amount(100, 5000))
            .RuleFor(f => f.DatePlaced, f => f.Date.Recent(30))
            .RuleFor(f => f.DateResolved, f => f.Random.Bool(0.3f) ? f.Date.Soon(10) : null)
            .RuleFor(f => f.IsActive, (f, hold) => hold.DateResolved == null)
            .RuleFor(f => f.ResolutionNotes, (f, hold) => 
                hold.DateResolved != null ? f.Lorem.Sentence() : null)
            .FinishWith((f, hold) =>
            {
                hold.StudentId = shuffledStudents[index].Id;
                hold.Student = shuffledStudents[index];
                hold.PlacedByUserId = shuffledUsers[index].Id;
                hold.PlacedByUser = shuffledUsers[index];

                if (hold.DateResolved.HasValue && index < users.Count - 1)
                {
                    hold.ResolvedByUserId = users[index + 1].Id;
                    hold.ResolvedByUser = users[index + 1];
                }
                index++;
            });

        return faker.Generate(count);
    }
}