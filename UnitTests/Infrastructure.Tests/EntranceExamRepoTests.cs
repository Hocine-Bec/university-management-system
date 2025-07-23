using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class EntranceExamRepoTests
{
    private const int TestSeed = 127;
    private readonly List<EntranceExam> _testEntranceExams = EntranceExamFactory.CreateTestEntranceExams(seed: TestSeed);

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testEntranceExams);

    [Fact]
    public async Task GetListAsync_WhenEntitiesExist_ShouldReturnAllEntities()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EntranceExamRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
        result.Should().BeEquivalentTo(_testEntranceExams);
    }

    [Fact]
    public async Task GetListAsync_WhenNoEntitiesExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await InMemoryDbFactory.CreateAsync();
        var repo = new EntranceExamRepository(context);

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
        var repo = new EntranceExamRepository(context);
        var entranceExam = _testEntranceExams.First();
        var entranceExamId = entranceExam.Id;

        // Act
        var result = await repo.GetByIdAsync(entranceExamId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(entranceExam);
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityNotFound_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EntranceExamRepository(context);
        const int entranceExamId = 999;

        // Act
        var result = await repo.GetByIdAsync(entranceExamId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByIdAsync_WhenIdIsInvalid_ShouldReturnNull(int entranceExamId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EntranceExamRepository(context);

        // Act
        var result = await repo.GetByIdAsync(entranceExamId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenEntityIsValid_ShouldAddAndSaveEntity()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EntranceExamRepository(context);
        var newEntranceExam = new Faker<EntranceExam>()
            .RuleFor(e => e.ExamDate, f => f.Date.Future())
            .RuleFor(e => e.Score, f => f.Random.Decimal(0, 100))
            .RuleFor(e => e.MaxScore, 100)
            .RuleFor(e => e.PassingScore, 70)
            .RuleFor(e => e.IsPassed, (f, e) => e.Score >= e.PassingScore)
            .RuleFor(e => e.PaidFees, f => f.Finance.Amount(50, 50))
            .RuleFor(e => e.ExamStatus, f => f.PickRandom<ExamStatus>())
            .RuleFor(e => e.Notes, f => f.Lorem.Sentence())
            .UseSeed(456)
            .Generate();

        // Act
        await repo.AddAsync(newEntranceExam);
        var result = await context.EntranceExams.SingleOrDefaultAsync(e => e.Id == newEntranceExam.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newEntranceExam);
    }

    [Fact]
    public async Task AddAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EntranceExamRepository(context);

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
        var repo = new EntranceExamRepository(context);
        var entranceExamToUpdate = await context.EntranceExams.FirstAsync();
        entranceExamToUpdate.Notes = "Updated EntranceExam Notes";

        // Act
        await repo.UpdateAsync(entranceExamToUpdate);
        var result = await context.EntranceExams.FirstAsync(e => e.Id == entranceExamToUpdate.Id);

        // Assert
        result.Should().NotBeNull();
        result.Notes.Should().Be("Updated EntranceExam Notes");
    }

    [Fact]
    public async Task UpdateAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EntranceExamRepository(context);

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
        var repo = new EntranceExamRepository(context);
        var entranceExamToDelete = await context.EntranceExams.FirstAsync();

        // Act
        var deleted = await repo.DeleteAsync(entranceExamToDelete.Id);
        var result = await context.EntranceExams.FirstOrDefaultAsync(e => e.Id == entranceExamToDelete.Id);

        // Assert
        deleted.Should().BeTrue();
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityNotFound_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EntranceExamRepository(context);
        const int entranceExamId = 999;

        // Act
        var result = await repo.DeleteAsync(entranceExamId);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DeleteAsync_WhenIdIsInvalid_ShouldReturnFalse(int entranceExamId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EntranceExamRepository(context);

        // Act
        var result = await repo.DeleteAsync(entranceExamId);

        // Assert
        result.Should().BeFalse();
    }
}
