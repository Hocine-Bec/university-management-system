using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.Common;

public static class EnrollmentFactory
{
    public static List<Enrollment> CreateTestEnrollments(
        int count, 
        List<Student> students,
        List<Program> programs,
        List<ServiceApplication> applications,
        int? seed = null)
    {
        if (count > students.Count || count > programs.Count || count > applications.Count)
            throw new ArgumentException("Cannot create more enrollments than available related entities");

        var faker = new Faker<Enrollment>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        var shuffledStudents = students.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var shuffledPrograms = programs.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var shuffledApplications = applications.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        
        var index = 0;

        faker.RuleFor(e => e.Id, f => f.IndexFaker + 1)
            .RuleFor(e => e.EnrollmentDate, f => f.Date.Past(2))
            .RuleFor(e => e.ActualGradDate, (f, e) => 
                f.Random.Bool(0.3f) ? f.Date.Between(e.EnrollmentDate, e.EnrollmentDate.AddYears(4)) : null)
            .RuleFor(e => e.Status, f => f.PickRandom<EnrollmentStatus>())
            .RuleFor(e => e.Notes, f => f.Random.Bool(0.7f) ? f.Lorem.Sentence() : null)
            .FinishWith((f, e) => 
            {
                e.StudentId = shuffledStudents[index].Id;
                e.Student = shuffledStudents[index];
                e.ProgramId = shuffledPrograms[index].Id;
                e.Program = shuffledPrograms[index];
                e.ServiceApplicationId = shuffledApplications[index].Id;
                e.ServiceApplication = shuffledApplications[index];
                index++;
            });

        return faker.Generate(count);
    }
}