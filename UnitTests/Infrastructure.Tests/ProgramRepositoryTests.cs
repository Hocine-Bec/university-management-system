using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class ProgramRepositoryTests
{
    private const int TestSeed = 125;
    private readonly List<Program> _testPrograms;

    public ProgramRepositoryTests()
    {
        _testPrograms = ProgramFactory.CreateTestPrograms(seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testPrograms);

    [Fact]
    public async Task GetListAsync_WhenEntitiesExist_ShouldReturnAllEntities()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
        result.Should().BeEquivalentTo(_testPrograms);
    }

    [Fact]
    public async Task GetListAsync_WhenNoEntitiesExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await InMemoryDbFactory.CreateAsync();
        var repo = new ProgramRepository(context);

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
        var repo = new ProgramRepository(context);
        var program = _testPrograms.First();
        var programId = program.Id;

        // Act
        var result = await repo.GetByIdAsync(programId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(program);
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityNotFound_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);
        const int programId = 999;

        // Act
        var result = await repo.GetByIdAsync(programId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByIdAsync_WhenIdIsInvalid_ShouldReturnNull(int programId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);

        // Act
        var result = await repo.GetByIdAsync(programId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByCodeAsync_WhenEntityExists_ShouldReturnCorrectEntity()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);
        var program = _testPrograms.First();
        var programCode = program.Code;

        // Act
        var result = await repo.GetByCodeAsync(programCode);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(program);
    }

    [Fact]
    public async Task GetByCodeAsync_WhenEntityNotFound_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);
        const string programCode = "P999";

        // Act
        var result = await repo.GetByCodeAsync(programCode);

        // Assert
        result.Should().BeNull();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetByCodeAsync_WhenCodeIsInvalid_ShouldReturnNull(string programCode)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);

        // Act
        var result = await repo.GetByCodeAsync(programCode);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenEntityIsValid_ShouldAddAndSaveEntity()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);
        var newProgram = new Faker<Program>()
            .RuleFor(p => p.Code, f => f.Random.AlphaNumeric(5).ToUpper())
            .RuleFor(p => p.Name, f => f.Commerce.Department())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.MinimumAge, f => f.Random.Int(18, 25))
            .RuleFor(p => p.Duration, f => f.Random.Int(2, 5))
            .RuleFor(p => p.Fees, f => f.Finance.Amount(1000, 5000))
            .RuleFor(p => p.IsActive, f => f.Random.Bool())
            .UseSeed(456)
            .Generate();
        
        // Act
        await repo.AddAsync(newProgram);
        var result = await context.Programs.SingleOrDefaultAsync(p => p.Code == newProgram.Code);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newProgram);
    }

    [Fact]
    public async Task AddAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);

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
        var repo = new ProgramRepository(context);
        var programToUpdate = await context.Programs.FirstAsync();
        programToUpdate.Name = "Updated Program Name";

        // Act
        await repo.UpdateAsync(programToUpdate);
        var result = await context.Programs.FirstAsync(p => p.Id == programToUpdate.Id);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Updated Program Name");
    }

    [Fact]
    public async Task UpdateAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);

        // Act
        var act = async () => await repo.UpdateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Fact]
    public async Task GetByCodeAsync_WhenCaseMismatch_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);
        var program = _testPrograms.First();
        var programCode = program.Code.ToLower(); // Case mismatch

        // Act
        var result = await repo.GetByCodeAsync(programCode);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DoesExistAsync_WhenEntityExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);
        var program = _testPrograms.First();
        var programCode = program.Code;

        // Act
        var result = await repo.DoesExistAsync(programCode);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesExistAsync_WhenEntityDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);
        const string programCode = "P999";

        // Act
        var result = await repo.DoesExistAsync(programCode);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task DoesExistAsync_WhenCodeIsInvalid_ShouldReturnFalse(string programCode)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);

        // Act
        var result = await repo.DoesExistAsync(programCode);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);
        var programToDelete = await context.Programs.FirstAsync();

        // Act
        var deleted = await repo.DeleteAsync(programToDelete.Code);
        var result = await context.Programs.FirstOrDefaultAsync(p => p.Id == programToDelete.Id);

        // Assert
        deleted.Should().BeTrue();
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityNotFound_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);
        const string programCode = "P999";

        // Act
        var result = await repo.DeleteAsync(programCode);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task DeleteAsync_WhenCodeIsInvalid_ShouldReturnFalse(string programCode)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProgramRepository(context);

        // Act
        var result = await repo.DeleteAsync(programCode);

        // Assert
        result.Should().BeFalse();
    }
}
