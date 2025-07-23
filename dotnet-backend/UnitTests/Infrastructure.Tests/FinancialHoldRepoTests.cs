using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class FinancialHoldRepoTests
{
    private const int TestSeed = 707;
    private readonly List<Country> _testCountries;
    private readonly List<Person> _testPeople;
    private readonly List<Student> _testStudents;
    private readonly List<User> _testUsers;
    private readonly List<FinancialHold> _testFinancialHolds;

    public FinancialHoldRepoTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(10, _testCountries, seed: TestSeed);
        _testStudents = StudentFactory.CreateTestStudents(5, _testPeople, seed: TestSeed);
        _testUsers = UserFactory.CreateTestUsers(5, _testPeople, seed: TestSeed);
        _testFinancialHolds = FinancialHoldFactory.CreateTestFinancialHolds(
            5, _testStudents, _testUsers, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testCountries, _testPeople, _testStudents,
            _testUsers, _testFinancialHolds);
    
    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();
    

    [Fact]
    public async Task GetListAsync_WhenFinancialHoldsExist_ShouldReturnAllHolds()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testFinancialHolds.Count);
        result.Should().BeEquivalentTo(_testFinancialHolds, options => 
            options.Excluding(f => f.Student)
                  .Excluding(f => f.PlacedByUser)
                  .Excluding(f => f.ResolvedByUser));
    }

    [Fact]
    public async Task GetListAsync_WhenNoFinancialHoldsExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new FinancialHoldRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenFinancialHoldExists_ShouldReturnHold()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);
        var expectedHold = _testFinancialHolds.First();

        // Act
        var result = await repo.GetByIdAsync(expectedHold.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedHold, options => 
            options.Excluding(f => f.Student)
                  .Excluding(f => f.PlacedByUser)
                  .Excluding(f => f.ResolvedByUser));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(99999)]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull(int invalidId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);

        // Act
        var result = await repo.GetByIdAsync(invalidId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WithValidFinancialHold_ShouldAddAndReturnId()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);
        var student = _testStudents.Last();
        var user = _testUsers.Last();

        var newHold = new FinancialHold
        {
            Reason = "Library fine overdue",
            HoldAmount = 50.00m,
            DatePlaced = DateTime.Now,
            IsActive = true,
            StudentId = student.Id,
            Student = null!,
            PlacedByUserId = user.Id,
            PlacedByUser = null!
        };

        // Act
        var resultId = await repo.AddAsync(newHold);
        var result = await context.FinancialHolds.FindAsync(resultId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().BePositive();
        result.Reason.Should().Be(newHold.Reason);
    }

    [Fact]
    public async Task AddAsync_WithNullFinancialHold_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.AddAsync(null!));
    }
    
    [Fact]
    public async Task UpdateAsync_WithValidChanges_ShouldUpdateSuccessfully()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);
        var hold = await context.FinancialHolds.FirstAsync();
        var originalReason = hold.Reason;
        hold.Reason = "Updated reason";

        // Act
        var result = await repo.UpdateAsync(hold);
        var updated = await context.FinancialHolds.FindAsync(hold.Id);

        // Assert
        result.Should().BeTrue();
        updated!.Reason.Should().NotBe(originalReason);
        updated.Reason.Should().Be("Updated reason");
    }

    [Fact]
    public async Task UpdateAsync_WithNullFinancialHold_ShouldThrow()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpdateAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);
        var invalidHold = new FinancialHold
        {
            Id = -1,
            Reason = "Invalid Reason",
            Student = null!,
            PlacedByUser = null!
        };

        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => repo.UpdateAsync(invalidHold));
    }

    [Fact]
    public async Task DeleteAsync_WithExistingId_ShouldRemoveAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);
        var holdId = _testFinancialHolds.First().Id;

        // Act
        var result = await repo.DeleteAsync(holdId);
        var deleted = await context.FinancialHolds.FindAsync(holdId);

        // Assert
        result.Should().BeTrue();
        deleted.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(99999)]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse(int invalidId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);

        // Act
        var result = await repo.DeleteAsync(invalidId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetByStudentIdAsync_WithExistingStudent_ShouldReturnHolds()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);
        var studentId = _testFinancialHolds.First().StudentId;
        var expectedCount = _testFinancialHolds.Count(f => f.StudentId == studentId);

        // Act
        var result = await repo.GetByStudentIdAsync(studentId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(expectedCount);
    }

    [Fact]
    public async Task GetByStudentIdAsync_WithStudentWithoutHolds_ShouldReturnEmpty()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new FinancialHoldRepository(context);
        var newStudent = _testStudents.First();
        await context.Students.AddAsync(newStudent);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByStudentIdAsync(newStudent.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByStudentIdAsync_WithInvalidStudentId_ShouldReturnEmpty(int invalidId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);

        // Act
        var result = await repo.GetByStudentIdAsync(invalidId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_WhenResolvingHold_ShouldUpdateAllFields()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new FinancialHoldRepository(context);
        var resolvingUser = _testUsers.Last();
        var hold = await context.FinancialHolds.FirstAsync(f => f.IsActive);
        
        var resolutionTime = DateTime.Now;
        hold.DateResolved = resolutionTime;
        hold.IsActive = false;
        hold.ResolutionNotes = "Paid in full";
        hold.ResolvedByUserId = resolvingUser.Id;

        // Act
        await repo.UpdateAsync(hold);
        var updated = await context.FinancialHolds.FindAsync(hold.Id);

        // Assert
        updated!.DateResolved.Should().BeCloseTo(resolutionTime, TimeSpan.FromSeconds(1));
        updated.IsActive.Should().BeFalse();
        updated.ResolutionNotes.Should().NotBeNull();
        updated.ResolvedByUserId.Should().Be(resolvingUser.Id);
    }
}