using Bogus;
using Domain.Entities;
using Domain.Enums;
using Person = Domain.Entities.Person;

namespace UnitTests.Common;

public static class StudentFactory
{
    public static List<Student> CreateTestStudents(int count, List<Person> people, int? seed = null)
    {
        if (count > people.Count)
            throw new ArgumentException("Cannot create more students than available people");
            
        var faker = new Faker<Student>();
        
        if (seed.HasValue)
            faker.UseSeed(seed.Value);
            
        var shuffledPeople = people.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var personIndex = 0;
        
        faker.RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(s => s.StudentNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(s => s.StudentStatus, f => f.PickRandom<StudentStatus>())
            .RuleFor(s => s.Notes, f => f.Lorem.Sentence())
            .FinishWith((f, s) =>
            {
                // Assign unique person to each student
                var person = shuffledPeople[personIndex++];
                s.PersonId = person.Id;
            });

        return faker.Generate(count);
    }
}