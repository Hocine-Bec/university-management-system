using System;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using Person = Domain.Entities.Person;

namespace UnitTests.Common;

public static class ProfessorFactory
{
    public static List<Professor> CreateTestProfessors(int count, List<Person> people, int? seed = null)
    {
        if (count > people.Count)
            throw new ArgumentException("Cannot create more professors than available people");

        var faker = new Faker<Professor>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        var shuffledPeople = people.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var personIndex = 0;

        faker.RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.EmployeeNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(p => p.AcademicRank, f => f.PickRandom<AcademicRank>())
            .RuleFor(p => p.HireDate, f => f.Date.Past(10, DateTime.Now))
            .RuleFor(p => p.Specialization, f => f.Commerce.Department())
            .RuleFor(p => p.OfficeLocation, f => f.Address.SecondaryAddress())
            .RuleFor(p => p.Salary, f => Math.Round(f.Finance.Amount(50000, 120000), 2))
            .RuleFor(p => p.IsActive, f => f.Random.Bool())
            .FinishWith((f, p) =>
            {
                var person = shuffledPeople[personIndex++];
                p.PersonId = person.Id;
                p.Person = person;
            });

        return faker.Generate(count);
    }
}
