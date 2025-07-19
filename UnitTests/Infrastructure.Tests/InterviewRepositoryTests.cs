using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using UnitTests.Common;
using UnitTests.Helpers;
using Person = Domain.Entities.Person;

namespace UnitTests.Infrastructure.Tests;

public class InterviewRepositoryTests
{
    private const int TestSeed = 12345;

    private readonly List<Person> _testPeople;
    private readonly List<Country> _testCountries;
    private readonly List<Professor> _testProfessors;
    private readonly List<Interview> _testInterviews;

    public InterviewRepositoryTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(10, _testCountries, seed: TestSeed);
        _testProfessors = ProfessorFactory.CreateTestProfessors(5, _testPeople, seed: TestSeed);
        _testInterviews = InterviewFactory.CreateTestInterviews(5, _testProfessors, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testPeople, _testProfessors, _testInterviews);

    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();

    [Fact]
    public async Task GetListAsync_WhenEntitiesExist_ShouldReturnAllEntities()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new InterviewRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testInterviews.Count);
        result.Should().BeEquivalentTo(_testInterviews, options => options
            .Excluding(x => x.Professor)); // Exclude navigation property if not loaded by default GetListAsync
    }

    [Fact]
    public async Task GetListAsync_WhenNoEntitiesExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new InterviewRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityExists_ShouldReturnCorrectEntity()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new InterviewRepository(context);
        var expectedInterview = _testInterviews.First();

        // Act
        var result = await repo.GetByIdAsync(expectedInterview.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedInterview, options => options
            .Excluding(x => x.Professor.Person.Country)); // Exclude nested navigation properties
        result!.Professor.Should().NotBeNull();
        result.Professor!.Id.Should().Be(expectedInterview.ProfessorId);
        result.Professor.Person.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new InterviewRepository(context);
        var nonExistentId = _testInterviews.Max(x => x.Id) + 1;

        // Act
        var result = await repo.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByIdAsync_WhenIdIsInvalid_ShouldReturnNull(int id)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new InterviewRepository(context);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenEntityIsValid_ShouldAddAndSaveEntity()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new InterviewRepository(context);
        var professor = _testProfessors.Last();
        var newInterview = new Faker<Interview>()
            .RuleFor(x => x.ScheduledDate, f => f.Date.Future(1))
            .RuleFor(x => x.StartTime, (f, o) => o.ScheduledDate.AddHours(f.Random.Int(9, 16)))
            .RuleFor(x => x.EndTime, (f, o) => o.StartTime.HasValue ? o.StartTime.Value.AddHours(f.Random.Int(1, 2)) : (DateTime?)null)
            .RuleFor(x => x.IsApproved, f => f.Random.Bool())
            .RuleFor(x => x.PaidFees, f => f.Finance.Amount(30, 50))
            .RuleFor(x => x.Notes, f => f.Lorem.Sentence())
            .RuleFor(x => x.Recommendation, f => f.PickRandom("Strongly Recommended", "Recommended", "Not Recommended", "Conditional Recommendation"))
            .RuleFor(x => x.ProfessorId, professor.Id)
            .UseSeed(456)
            .Generate();
        
        // Act
        var newId = await repo.AddAsync(newInterview);
        var addedInterview = await repo.GetByIdAsync(newId);
        
        // Assert
        addedInterview.Should().NotBeNull();
        addedInterview.Id.Should().BeGreaterThan(0);
        addedInterview.Should().NotBeNull();
        addedInterview.Should().BeEquivalentTo(newInterview, options => options.Excluding(x => x.Id).Excluding(x => x.Professor));
    }

    [Fact]
    public async Task AddAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new InterviewRepository(context);

        // Act
        var act = async () => await repo.AddAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_WhenEntityExists_ShouldUpdateAndSaveChanges()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new InterviewRepository(context);
        var interviewToUpdate = _testInterviews.First();
        interviewToUpdate.Notes = "Updated notes for interview";
        interviewToUpdate.IsApproved = !interviewToUpdate.IsApproved;

        // Act
        var result = await repo.UpdateAsync(interviewToUpdate);
        var updatedInterview = await repo.GetByIdAsync(interviewToUpdate.Id);

        // Assert
        result.Should().BeTrue();
        updatedInterview.Should().NotBeNull();
        updatedInterview!.Notes.Should().Be("Updated notes for interview");
        updatedInterview.IsApproved.Should().Be(interviewToUpdate.IsApproved);
    }

    [Fact]
    public async Task UpdateAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new InterviewRepository(context);

        // Act
        var act = async () => await repo.UpdateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new InterviewRepository(context);
        var interviewToDelete = _testInterviews.First();

        // Act
        var result = await repo.DeleteAsync(interviewToDelete.Id);
        await context.SaveChangesAsync(); // Save changes to persist

        // Assert
        result.Should().BeTrue();
        var deletedInterview = await context.Interviews.FindAsync(interviewToDelete.Id);
        deletedInterview.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new InterviewRepository(context);
        var nonExistentId = _testInterviews.Max(x => x.Id) + 1;

        // Act
        var result = await repo.DeleteAsync(nonExistentId);
        await context.SaveChangesAsync(); // Save changes to persist

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DeleteAsync_WhenIdIsInvalid_ShouldReturnFalse(int id)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new InterviewRepository(context);

        // Act
        var result = await repo.DeleteAsync(id);
        await context.SaveChangesAsync(); // Save changes to persist

        // Assert
        result.Should().BeFalse();
    }
}
