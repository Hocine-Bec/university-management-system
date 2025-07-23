using Bogus;
using Domain.Entities;

namespace UnitTests.Common;

public static class PrerequisiteFactory
{
    public static List<Prerequisite> CreateTestPrerequisites(int count, List<Course> courses, int? seed = null)
    {
        if (count > courses.Count * (courses.Count - 1))
            throw new ArgumentException("Not enough unique course combinations");

        var faker = new Faker<Prerequisite>();

        if (seed.HasValue)
            faker.UseSeed(seed.Value);

        var courseCombinations = GetUniqueCoursePairs(courses);
        var combinationsUsed = 0;

        faker.RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.MinimumGrade, f => Math.Round(f.Random.Decimal(2.0m, 4.0m), 2))
            .FinishWith((f, p) => 
            {
                var (course, prereqCourse) = courseCombinations[combinationsUsed++];
                p.CourseId = course.Id;
                p.Course = course;
                p.PrerequisiteCourseId = prereqCourse.Id;
                p.PrerequisiteCourse = prereqCourse;
            });

        return faker.Generate(count);
    }

    private static List<(Course Course, Course PrerequisiteCourse)> GetUniqueCoursePairs(List<Course> courses)
    {
        var pairs = new List<(Course, Course)>();
        var random = new Random();
    
        // Shuffle the courses to get random pairs
        var shuffled = courses.OrderBy(x => random.Next()).ToList();

        for (int i = 0; i < shuffled.Count; i++)
        {
            // Ensure we don't pair a course with itself
            var prereqIndex = (i + 1) % shuffled.Count;
            pairs.Add((shuffled[i], shuffled[prereqIndex]));
        }

        return pairs;
    }
}