using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class SemesterRepoTests
{
    private const int TestSeed = 202;
    private readonly List<Semester> _testSemesters = SemesterFactory.CreateTestSemesters(5, seed: TestSeed);

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testSemesters);
    
    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();

    [Fact]
    public async Task GetListAsync_WhenSemestersExist_ShouldReturnAllSemesters()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testSemesters.Count);
        result.Should().BeEquivalentTo(_testSemesters);
    }

    [Fact]
    public async Task GetListAsync_WhenNoSemestersExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new SemesterRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenSemesterExists_ShouldReturnCorrectSemester()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);
        var expectedSemester = _testSemesters.First();

        // Act
        var result = await repo.GetByIdAsync(expectedSemester.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedSemester);
    }

    [Fact]
    public async Task GetByIdAsync_WhenSemesterDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);
        const int nonExistentId = -1;

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
        var repo = new SemesterRepository(context);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByTermCodeAsync_WhenSemesterExists_ShouldReturnCorrectSemester()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);
        var expectedSemester = _testSemesters.First();

        // Act
        var result = await repo.GetByTermCodeAsync(expectedSemester.TermCode);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedSemester);
    }

    [Fact]
    public async Task GetByTermCodeAsync_WhenSemesterDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);
        const string nonExistentTermCode = "nonexistent";

        // Act
        var result = await repo.GetByTermCodeAsync(nonExistentTermCode);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetByTermCodeAsync_WhenTermCodeIsInvalid_ShouldReturnNull(string termCode)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);

        // Act
        var result = await repo.GetByTermCodeAsync(termCode);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenSemesterIsValid_ShouldAddAndSaveSemester()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);
        var newSemester = new Faker<Semester>()
            .RuleFor(s => s.TermCode, f => $"2024{f.PickRandom("FA", "SP", "SU")}")
            .RuleFor(s => s.Term, f => f.PickRandom("Fall", "Spring", "Summer"))
            .RuleFor(s => s.Year, 2024)
            .RuleFor(s => s.StartDate, f => f.Date.Between(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31)))
            .RuleFor(s => s.EndDate, (f, s) => s.StartDate.AddMonths(4))
            .RuleFor(s => s.RegStartsAt, (f, s) => s.StartDate.AddMonths(-2))
            .RuleFor(s => s.RegEndsAt, (f, s) => s.StartDate.AddDays(-7))
            .RuleFor(s => s.IsActive, true)
            .UseSeed(303)
            .Generate();

        // Act
        await repo.AddAsync(newSemester);
        var result = await context.Semesters
            .FirstOrDefaultAsync(s => s.TermCode == newSemester.TermCode);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newSemester);
    }

    [Fact]
    public async Task AddAsync_WhenSemesterIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);

        // Act
        var act = async () => await repo.AddAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_WhenSemesterExists_ShouldUpdateAndSaveChanges()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);
        var semesterToUpdate = await context.Semesters.FirstAsync();
        var originalIsActive = semesterToUpdate.IsActive;
        semesterToUpdate.IsActive = !originalIsActive;

        // Act
        await repo.UpdateAsync(semesterToUpdate);
        var result = await context.Semesters.FindAsync(semesterToUpdate.Id);

        // Assert
        result.Should().NotBeNull();
        result!.IsActive.Should().NotBe(originalIsActive);
        result.IsActive.Should().Be(semesterToUpdate.IsActive);
    }

    [Fact]
    public async Task UpdateAsync_WhenSemesterIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);

        // Act
        var act = async () => await repo.UpdateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenSemesterExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);
        var semesterToDelete = _testSemesters.First();

        // Act
        var result = await repo.DeleteAsync(semesterToDelete.Id);
        var deletedSemester = await repo.GetByIdAsync(semesterToDelete.Id);

        // Assert
        result.Should().BeTrue();
        deletedSemester.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenSemesterDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new SemesterRepository(context);
        const int nonExistentId = -1;

        // Act
        var result = await repo.DeleteAsync(nonExistentId);

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
        var repo = new SemesterRepository(context);

        // Act
        var result = await repo.DeleteAsync(id);

        // Assert
        result.Should().BeFalse();
    }
}