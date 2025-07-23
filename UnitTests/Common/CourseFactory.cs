
using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class CourseFactory
{
    public static List<Course> CreateTestCourses(int count = 5, int? seed = 123)
    {
        var faker = new Faker<Course>();
        
        if (seed.HasValue)
            faker.UseSeed(seed.Value);
                
            faker.RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.Code, f => $"C{f.Random.Number(100, 999)}")
            .RuleFor(c => c.Title, f => f.Lorem.Sentence(3))
            .RuleFor(c => c.Description, f => f.Lorem.Paragraph())
            .RuleFor(c => c.CreditHours, f => f.Random.Int(1, 4))
            .RuleFor(c => c.IsActive, f => f.Random.Bool());

        return faker.Generate(count);
    }
}
