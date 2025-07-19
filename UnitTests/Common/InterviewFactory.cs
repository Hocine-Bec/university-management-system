using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class InterviewFactory
{
    public static List<Interview> CreateTestInterviews(int count, List<Professor> professors, int? seed = null)
    {
        if (professors == null || professors.Count == 0)
        {
            throw new ArgumentException("Professors list cannot be null or empty.", nameof(professors));
        }

        var faker = new Faker<Interview>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        faker.RuleFor(x => x.Id, f => f.IndexFaker + 1)
            .RuleFor(x => x.ScheduledDate, f => f.Date.Future(1))
            .RuleFor(x => x.StartTime, (f, o) => o.ScheduledDate.AddHours(f.Random.Int(9, 16)))
            .RuleFor(x => x.EndTime, (f, o) => o.StartTime.HasValue ? o.StartTime.Value.AddHours(f.Random.Int(1, 2)) : (DateTime?)null)
            .RuleFor(x => x.IsApproved, f => f.Random.Bool())
            .RuleFor(x => x.PaidFees, f => f.Finance.Amount(30, 50))
            .RuleFor(x => x.Notes, f => f.Lorem.Sentence())
            .RuleFor(x => x.Recommendation, f => f.PickRandom("Strongly Recommended", "Recommended", "Not Recommended", "Conditional Recommendation"))
            .RuleFor(x => x.ProfessorId, f => f.PickRandom(professors).Id)
            .RuleFor(x => x.Professor, (f, o) => professors.Find(p => p.Id == o.ProfessorId));

        return faker.Generate(count);
    }
}
