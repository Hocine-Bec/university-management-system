using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class GradeRepoTests
{
    private const int TestSeed = 808;
    private readonly List<Country> _testCountries;
    private readonly List<Person> _testPeople;
    private readonly List<Student> _testStudents;
    private readonly List<Course> _testCourses;
    private readonly List<Semester> _testSemesters;
    private readonly List<Section> _testSections;
    private readonly List<Registration> _testRegistrations;
    private readonly List<Grade> _testGrades;

    public GradeRepoTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(10, _testCountries, seed: TestSeed);
        _testStudents = StudentFactory.CreateTestStudents(8, _testPeople, seed: TestSeed);
        _testCourses = CourseFactory.CreateTestCourses(5, seed: TestSeed);
        _testSemesters = SemesterFactory.CreateTestSemesters(5, seed: TestSeed);
        _testSections =
            SectionFactory.CreateTestSections(5, _testCourses, _testSemesters, new List<Professor>(), seed: TestSeed);

        var testUsers = UserFactory.CreateTestUsers(3, _testPeople, seed: TestSeed);
        _testRegistrations = RegistrationFactory.CreateTestRegistrations(
            5, _testStudents, _testSections, _testSemesters, testUsers, seed: TestSeed);

        _testGrades = GradeFactory.CreateTestGrades(
            5, _testStudents, _testCourses, _testSemesters, _testRegistrations, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(
            _testCountries, _testPeople, _testStudents,
            _testCourses, _testSemesters, _testSections,
            _testRegistrations, _testGrades);

    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();

    private void SetupConstraintValidation(AppDbContext context)
    {
        context.SavingChanges += (sender, args) =>
        {
            // Validate foreign keys
            foreach (var entry in context.ChangeTracker.Entries<Grade>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    var grade = entry.Entity;

                    // Validate Student exists
                    if (!context.Students.Any(s => s.Id == grade.StudentId))
                        throw new DbUpdateException(
                            $"Violation of FOREIGN KEY constraint 'FK_Grades_Students_StudentId'");

                    // Validate Course exists
                    if (!context.Courses.Any(c => c.Id == grade.CourseId))
                        throw new DbUpdateException(
                            $"Violation of FOREIGN KEY constraint 'FK_Grades_Courses_CourseId'");

                    // Validate Semester exists
                    if (!context.Semesters.Any(s => s.Id == grade.SemesterId))
                        throw new DbUpdateException(
                            $"Violation of FOREIGN KEY constraint 'FK_Grades_Semesters_SemesterId'");

                    // Validate Registration exists if specified
                    if (grade.RegistrationId.HasValue &&
                        !context.Registrations.Any(r => r.Id == grade.RegistrationId))
                        throw new DbUpdateException(
                            $"Violation of FOREIGN KEY constraint 'FK_Grades_Registrations_RegistrationId'");

                    // Validate score range
                    if (grade.Score < 0 || grade.Score > 100)
                        throw new DbUpdateException($"Violation of CHECK constraint 'CK_Grades_Score_Range'");
                }
            }
        };
    }

    [Fact]
    public async Task GetByIdAsync_WhenGradeExists_ShouldReturnGradeWithNavigations()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new GradeRepository(context);
        var expectedGrade = _testGrades.First();

        // Act
        var result = await repo.GetByIdAsync(expectedGrade.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedGrade, options =>
            options.Excluding(g => g.Registration)
                .Excluding(g => g.Student.Person));
    }

    [Fact]
    public async Task GetByIdAsync_WhenGradeDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new GradeRepository(context);
        const int nonExistentId = -1;

        // Act
        var result = await repo.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByStudentIdAsync_ShouldReturnGradesForStudent()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new GradeRepository(context);
        var studentId = _testGrades.First().StudentId;
        var expectedGrades = _testGrades.Where(g => g.StudentId == studentId).ToList();

        // Act
        var result = await repo.GetByStudentIdAsync(studentId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(expectedGrades.Count);
        result.Should().BeEquivalentTo(expectedGrades, options =>
            options.Excluding(g => g.Registration)
                .Excluding(g => g.Student.Person));
    }

    [Fact]
    public async Task GetByStudentIdAsync_WhenStudentHasNoGrades_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new GradeRepository(context);
        var studentWithoutGrades = _testStudents.First(s =>
            !_testGrades.Any(g => g.StudentId == s.Id));

        // Act
        var result = await repo.GetByStudentIdAsync(studentWithoutGrades.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByCourseIdAsync_ShouldReturnGradesForCourse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new GradeRepository(context);
        var courseId = _testGrades.First().CourseId;
        var expectedGrades = _testGrades.Where(g => g.CourseId == courseId).ToList();

        // Act
        var result = await repo.GetByCourseIdAsync(courseId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(expectedGrades.Count);
        result.Should().BeEquivalentTo(expectedGrades, options =>
            options.Excluding(g => g.Registration)
                .Excluding(g => g.Student.Person));
    }

    [Fact]
    public async Task AddAsync_WhenGradeIsValid_ShouldAddAndSaveGrade()
    {
        // Arrange
        await using var context = await GetDbContext();
        SetupConstraintValidation(context);
        var repo = new GradeRepository(context);

        var newGrade = new Grade
        {
            Score = 85.5m,
            DateRecorded = DateTime.UtcNow,
            Comments = "Good performance",
            StudentId = _testStudents.Last().Id,
            CourseId = _testCourses.Last().Id,
            SemesterId = _testSemesters.Last().Id,
            RegistrationId = _testRegistrations.Last().Id,
            Student = null!,
            Course = null!,
            Semester = null!
        };

        // Act
        await repo.AddAsync(newGrade);
        var result = await context.Grades
            .FirstOrDefaultAsync(g => g.Score == 85.5m && g.StudentId == newGrade.StudentId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newGrade, options =>
            options.Excluding(g => g.Id)
                .Excluding(g => g.Student)
                .Excluding(g => g.Course)
                .Excluding(g => g.Semester)
                .Excluding(g => g.Registration));
    }

    [Fact]
    public async Task AddAsync_WhenStudentDoesNotExist_ShouldThrowException()
    {
        // Arrange
        await using var context = await GetDbContext();
        SetupConstraintValidation(context);
        var repo = new GradeRepository(context);

        var invalidGrade = new Grade
        {
            Score = 75,
            StudentId = -1, // Invalid student
            CourseId = _testCourses.First().Id,
            SemesterId = _testSemesters.First().Id,
            Student = null!,
            Course = null!,
            Semester = null!
        };

        // Act
        Func<Task> act = async () => await repo.AddAsync(invalidGrade);

        // Assert
        await act.Should().ThrowAsync<DbUpdateException>()
            .WithMessage("*FK_Grades_Students_StudentId*");
    }

    [Fact]
    public async Task AddAsync_WhenScoreIsInvalid_ShouldThrowException()
    {
        // Arrange
        await using var context = await GetDbContext();
        SetupConstraintValidation(context);
        var repo = new GradeRepository(context);

        var invalidGrade = new Grade
        {
            Score = 150, // Invalid score
            StudentId = _testStudents.First().Id,
            CourseId = _testCourses.First().Id,
            SemesterId = _testSemesters.First().Id,
            Student = null!,
            Course = null!,
            Semester = null!
        };

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () => await repo.AddAsync(invalidGrade));
    }

    [Fact]
    public async Task DoesExistAsync_WhenGradeExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new GradeRepository(context);
        var existingGrade = _testGrades.First();

        // Act
        var result = await repo.DoesExistAsync(
            existingGrade.StudentId,
            existingGrade.CourseId,
            existingGrade.SemesterId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesExistAsync_WhenGradeDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new GradeRepository(context);
        var studentId = _testStudents.Last().Id;
        var courseId = _testCourses.Last().Id;
        var semesterId = _testSemesters.Last().Id;

        // Ensure no grade exists for this combination
        if (_testGrades.Any(g => g.StudentId == studentId &&
                                 g.CourseId == courseId &&
                                 g.SemesterId == semesterId))
        {
            _testGrades.RemoveAll(g => g.StudentId == studentId &&
                                       g.CourseId == courseId &&
                                       g.SemesterId == semesterId);
        }

        // Act
        var result = await repo.DoesExistAsync(studentId, courseId, semesterId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_WhenGradeExists_ShouldUpdateSuccessfully()
    {
        // Arrange
        await using var context = await GetDbContext();
        SetupConstraintValidation(context);
        var repo = new GradeRepository(context);
        var existingGrade = _testGrades.First();
        var originalScore = existingGrade.Score;
        var updatedScore = originalScore + 5;

        // Act
        existingGrade.Score = updatedScore;
        var result = await repo.UpdateAsync(existingGrade);
        var updatedEntity = await repo.GetByIdAsync(existingGrade.Id);

        // Assert
        result.Should().BeTrue();
        updatedEntity.Should().NotBeNull();
        updatedEntity!.Score.Should().Be(updatedScore);
    }

    [Fact]
    public async Task UpdateAsync_WhenGradeDoesNotExist_ShouldThrowException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new GradeRepository(context);
        var nonExistentGrade = new Grade
        {
            Id = -1,
            Student = null!,
            Course = null!,
            Semester = null!
        };

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await repo.UpdateAsync(nonExistentGrade));
    }

    [Fact]
    public async Task UpdateAsync_WhenScoreIsInvalid_ShouldThrowException()
    {
        // Arrange
        await using var context = await GetDbContext();
        SetupConstraintValidation(context);
        var repo = new GradeRepository(context);
        var existingGrade = _testGrades.First();
        existingGrade.Score = 150; // Invalid score

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () => await repo.UpdateAsync(existingGrade));
    }

    [Fact]
    public async Task DeleteAsync_WhenGradeExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new GradeRepository(context);
        var gradeToDelete = _testGrades.First();

        // Act
        var result = await repo.DeleteAsync(gradeToDelete.Id);
        var deletedGrade = await repo.GetByIdAsync(gradeToDelete.Id);

        // Assert
        result.Should().BeTrue();
        deletedGrade.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenGradeDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new GradeRepository(context);
        const int nonExistentId = -1;

        // Act
        var result = await repo.DeleteAsync(nonExistentId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetListAsync_WhenGradesExist_ShouldReturnAllGrades()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new GradeRepository(context);

        // Act
        var result = await repo.GetListAsync();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testGrades.Count);
        result.Should().BeEquivalentTo(_testGrades, options =>
            options.Excluding(g => g.Registration)
                .Excluding(g => g.Student)
                .Excluding(g => g.Course)
                .Excluding(g => g.Semester));
    }

    [Fact]
    public async Task GetListAsync_WhenNoGradesExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new GradeRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    
}