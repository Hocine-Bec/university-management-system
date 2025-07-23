using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class RegistrationFactory
{
    public static List<Registration> CreateTestRegistrations(
        int count,
        List<Student> students,
        List<Section> sections,
        List<Semester> semesters,
        List<User> users,
        int? seed = null)
    {
        if (count > students.Count || count > sections.Count || count > semesters.Count)
        {
            throw new ArgumentException("Cannot create more registrations than available students, sections, or semesters");
        }

        var faker = new Faker<Registration>();

        if (seed.HasValue)
        {
            faker.UseSeed(seed.Value);
        }

        var shuffledStudents = students.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var shuffledSections = sections.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var shuffledSemesters = semesters.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var userPool = users.Where(u => u.IsActive).ToList();
        
        var index = 0;

        faker.RuleFor(r => r.Id, f => f.IndexFaker + 1)
            .RuleFor(r => r.RegistrationDate, f => f.Date.Recent(30))
            .RuleFor(r => r.RegistrationFees, f => f.Finance.Amount(100, 500))
            .FinishWith((f, r) =>
            {
                var student = shuffledStudents[index];
                var section = shuffledSections[index];
                var semester = shuffledSemesters[index];

                r.StudentId = student.Id;
                r.Student = student;
                
                r.SectionId = section.Id;
                r.Section = section;
                r.Section.Course = section.Course; // Ensure Course is set
                r.Section.Semester = section.Semester; // Ensure Semester is set
                
                r.SemesterId = semester.Id;
                r.Semester = semester;

                // 70% chance to have a processed by user
                if (userPool.Any() && f.Random.Bool(0.7f))
                {
                    var user = f.PickRandom(userPool);
                    r.ProcessedByUserId = user.Id;
                    r.ProcessedByUser = user;
                    r.ProcessedByUser.Person = user.Person; // Ensure Person is set
                }

                index++;
            });

        return faker.Generate(count);
    }
}