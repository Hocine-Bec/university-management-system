using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class SectionFactory
{
    public static List<Section> CreateTestSections(
        int count, 
        List<Course> courses,
        List<Semester> semesters,
        List<Professor> professors,
        int? seed = null)
    {
        if (count > courses.Count || count > semesters.Count)
            throw new ArgumentException("Cannot create more sections than available courses or semesters");

        var faker = new Faker<Section>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        var shuffledCourses = courses.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var shuffledSemesters = semesters.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
        var professorPool = professors.Where(p => p.IsActive).ToList();
        
        var courseIndex = 0;
        var semesterIndex = 0;

        faker.RuleFor(s => s.Id, f => f.IndexFaker + 1)
            .RuleFor(s => s.SectionNumber, (f, s) => $"{shuffledCourses[courseIndex].Code}-{f.Random.Number(1, 50)}")
            .RuleFor(s => s.MeetingDays, f => f.PickRandom("MWF", "TTh", "MW", "F", null))
            .RuleFor(s => s.StartTime, f => f.Date.BetweenTimeOnly(new TimeOnly(8, 0), new TimeOnly(18, 0)))
            .RuleFor(s => s.EndTime, (f, s) => s.StartTime?.AddHours(f.Random.Int(1, 3)))
            .RuleFor(s => s.Classroom, f => f.Address.BuildingNumber() + f.Random.AlphaNumeric(1).ToUpper())
            .RuleFor(s => s.MaxCapacity, f => f.Random.Int(15, 40) * 5)
            .RuleFor(s => s.CurrentEnrollment, f => f.Random.Bool(0.8f) ? f.Random.Int(0, 40) : null)
            .FinishWith((f, s) =>
            {
                s.CourseId = shuffledCourses[courseIndex].Id;
                s.Course = shuffledCourses[courseIndex];
                
                s.SemesterId = shuffledSemesters[semesterIndex].Id;
                s.Semester = shuffledSemesters[semesterIndex];

                if (professorPool.Count != 0 && f.Random.Bool(0.7f))
                {
                    var prof = f.PickRandom(professorPool);
                    s.ProfessorId = prof.Id;
                    s.Professor = prof;
                }

                courseIndex++;
                semesterIndex++;
            });

        return faker.Generate(count);
    }
}