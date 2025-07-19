
using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.Common;

public static class EntranceExamFactory
{
    public static List<EntranceExam> CreateTestEntranceExams(int count = 5, int? seed = 123)
    {
        var faker = new Faker<EntranceExam>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        faker.RuleFor(e => e.Id, f => f.IndexFaker + 1)
            .RuleFor(e => e.ExamDate, f => f.Date.Future())
            .RuleFor(e => e.Score, f => f.Random.Decimal(0, 100))
            .RuleFor(e => e.MaxScore, 100)
            .RuleFor(e => e.PassingScore, 70)
            .RuleFor(e => e.IsPassed, (f, e) => e.Score >= e.PassingScore)
            .RuleFor(e => e.PaidFees, f => f.Finance.Amount(50, 50))
            .RuleFor(e => e.ExamStatus, f => f.PickRandom<ExamStatus>())
            .RuleFor(e => e.Notes, f => f.Lorem.Sentence());
        
        return faker.Generate(count);
    }
}
