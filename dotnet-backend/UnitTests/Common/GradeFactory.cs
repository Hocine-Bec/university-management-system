using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class GradeFactory
{
    public static List<Grade> CreateTestGrades(
        int count,
        List<Student> students,
        List<Course> courses,
        List<Semester> semesters,
        List<Registration>? registrations = null,
        int? seed = null)
    {
        if (count > students.Count || count > courses.Count || count > semesters.Count)
            throw new ArgumentException("Cannot create more grades than available students, courses, or semesters");

        var faker = new Faker<Grade>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        var shuffledStudents = students.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var shuffledCourses = courses.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var shuffledSemesters = semesters.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var index = 0;

        faker.RuleFor(g => g.Id, f => f.IndexFaker + 1)
            .RuleFor(g => g.Score, f => Math.Round(f.Random.Decimal(0, 100), 2))
            .RuleFor(g => g.DateRecorded, f => f.Date.Recent())
            .RuleFor(g => g.Comments, f => f.Random.Bool(0.7f) ? f.Lorem.Sentence() : null)
            .FinishWith((f, g) =>
            {
                g.StudentId = shuffledStudents[index].Id;
                g.Student = shuffledStudents[index];
                g.CourseId = shuffledCourses[index].Id;
                g.Course = shuffledCourses[index];
                g.SemesterId = shuffledSemesters[index].Id;
                g.Semester = shuffledSemesters[index];
                
                if (registrations != null && registrations.Any())
                {
                    var registration = registrations.FirstOrDefault(r => 
                        r.StudentId == g.StudentId && 
                        r.Section.CourseId == g.CourseId &&
                        r.Section.SemesterId == g.SemesterId);
                    
                    if (registration != null)
                    {
                        g.RegistrationId = registration.Id;
                        g.Registration = registration;
                    }
                }
                
                index++;
            });

        return faker.Generate(count);
    }
}