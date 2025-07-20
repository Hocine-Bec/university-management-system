using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;
using Person = Domain.Entities.Person;

namespace UnitTests.Infrastructure.Tests;

public class SectionRepoTests
{
    private const int TestSeed = 404;
    private readonly List<Country> _testCountries;
    private readonly List<Person> _testPeople;
    private readonly List<Professor> _testProfessors;
    private readonly List<Course> _testCourses;
    private readonly List<Semester> _testSemesters;
    private readonly List<Section> _testSections;

    public SectionRepoTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(8, _testCountries, seed: TestSeed);
        _testProfessors = ProfessorFactory.CreateTestProfessors(5, _testPeople, seed: TestSeed);
        _testCourses = CourseFactory.CreateTestCourses(5, seed: TestSeed);
        _testSemesters = SemesterFactory.CreateTestSemesters(5, seed: TestSeed);
        _testSections = SectionFactory.CreateTestSections(5, _testCourses, _testSemesters, _testProfessors, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testCountries, _testPeople, _testProfessors, 
            _testCourses, _testSemesters, _testSections);
    
    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();

    [Fact]
    public async Task GetListAsync_WhenSectionsExist_ShouldReturnAllSections()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SectionRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testSections.Count);
        result.Should().BeEquivalentTo(_testSections, options => 
            options.Excluding(s => s.Course)
                  .Excluding(s => s.Semester)
                  .Excluding(s => s.Professor));
    }

    [Fact]
    public async Task GetListAsync_WhenNoSectionsExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new SectionRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenSectionExists_ShouldReturnCorrectSection()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SectionRepository(context);
        var expectedSection = _testSections.First();

        // Act
        var result = await repo.GetByIdAsync(expectedSection.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedSection, options => 
            options.Excluding(s => s.Course)
                  .Excluding(s => s.Semester)
                  .Excluding(s => s.Professor));
    }

    [Fact]
    public async Task GetBySectionNumberAsync_WhenSectionExists_ShouldReturnCorrectSection()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SectionRepository(context);
        var expectedSection = _testSections.First();

        // Act
        var result = await repo.GetBySectionNumberAsync(expectedSection.SectionNumber);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedSection, options => 
            options.Excluding(s => s.Course)
                  .Excluding(s => s.Semester)
                  .Excluding(s => s.Professor));
    }

    [Fact]
    public async Task GetBySectionNumberAsync_WhenSectionDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SectionRepository(context);
        const string nonExistentSectionNumber = "nonexistent";

        // Act
        var result = await repo.GetBySectionNumberAsync(nonExistentSectionNumber);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetBySectionNumberAsync_WhenSectionNumberIsInvalid_ShouldReturnNull(string sectionNumber)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SectionRepository(context);

        // Act
        var result = await repo.GetBySectionNumberAsync(sectionNumber);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenSectionIsValid_ShouldAddAndSaveSection()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SectionRepository(context);
        
        var course = _testCourses.Last();
        var semester = _testSemesters.Last();
        var professor = _testProfessors.First();

        var newSection = new Faker<Section>()
            .RuleFor(s => s.SectionNumber, f => $"{course.Code}-{f.Random.Number(51, 100)}")
            .RuleFor(s => s.MeetingDays, "MWF")
            .RuleFor(s => s.StartTime, new TimeOnly(10, 0))
            .RuleFor(s => s.EndTime, new TimeOnly(11, 30))
            .RuleFor(s => s.Classroom, "B101")
            .RuleFor(s => s.MaxCapacity, 30)
            .RuleFor(s => s.CurrentEnrollment, 0)
            .RuleFor(s => s.CourseId, course.Id)
            .RuleFor(s => s.SemesterId, semester.Id)
            .RuleFor(s => s.ProfessorId, professor.Id)
            .UseSeed(456)
            .Generate();

        // Act
        await repo.AddAsync(newSection);
        var result = await context.Sections
            .FirstOrDefaultAsync(s => s.SectionNumber == newSection.SectionNumber);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newSection, options => 
            options.Excluding(s => s.Course)
                  .Excluding(s => s.Semester)
                  .Excluding(s => s.Professor));
    }

    [Fact]
    public async Task DeleteAsync_BySectionNumber_WhenSectionExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SectionRepository(context);
        var sectionToDelete = _testSections.First();

        // Act
        var result = await repo.DeleteAsync(sectionToDelete.SectionNumber);
        var deletedSection = await repo.GetBySectionNumberAsync(sectionToDelete.SectionNumber);

        // Assert
        result.Should().BeTrue();
        deletedSection.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_BySectionNumber_WhenSectionDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SectionRepository(context);
        const string nonExistentSectionNumber = "nonexistent";

        // Act
        var result = await repo.DeleteAsync(nonExistentSectionNumber);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DoesExistAsync_WhenSectionExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SectionRepository(context);
        var existingSectionNumber = _testSections.First().SectionNumber;

        // Act
        var result = await repo.DoesExistAsync(existingSectionNumber);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesExistAsync_WhenSectionDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SectionRepository(context);
        const string nonExistentSectionNumber = "nonexistent";

        // Act
        var result = await repo.DoesExistAsync(nonExistentSectionNumber);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task DoesExistAsync_WhenSectionNumberIsInvalid_ShouldReturnFalse(string sectionNumber)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SectionRepository(context);

        // Act
        var result = await repo.DoesExistAsync(sectionNumber);

        // Assert
        result.Should().BeFalse();
    }
}