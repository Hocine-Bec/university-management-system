using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class SemesterFactory
{
    public static List<Semester> CreateTestSemesters(int count, int? seed = null)
    {
        var faker = new Faker<Semester>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        var terms = new[] { "Fall", "Spring", "Summer" };
        var currentYear = DateTime.Now.Year;

        faker.RuleFor(s => s.Id, f => f.IndexFaker + 1)
            .RuleFor(s => s.TermCode, f => $"{currentYear}{f.PickRandom(terms).Substring(0, 2)}")
            .RuleFor(s => s.Term, f => f.PickRandom(terms))
            .RuleFor(s => s.Year, f => f.Random.Int(currentYear - 5, currentYear + 1))
            .RuleFor(s => s.StartDate, (f, s) => f.Date.Between(
                new DateTime(s.Year, s.Term == "Summer" ? 5 : (s.Term == "Fall" ? 8 : 1), 1),
                new DateTime(s.Year, s.Term == "Summer" ? 7 : (s.Term == "Fall" ? 12 : 5), 1)))
            .RuleFor(s => s.EndDate, (f, s) => f.Date.Between(
                s.StartDate.AddMonths(3),
                s.StartDate.AddMonths(4)))
            .RuleFor(s => s.RegStartsAt, (f, s) => f.Date.Between(
                s.StartDate.AddMonths(-2),
                s.StartDate.AddMonths(-1)))
            .RuleFor(s => s.RegEndsAt, (f, s) => f.Date.Between(
                s.StartDate.AddDays(-14),
                s.StartDate.AddDays(-1)))
            .RuleFor(s => s.IsActive, f => f.Random.Bool(0.7f));

        return faker.Generate(count);
    }
}