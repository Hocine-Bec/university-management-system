using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class DocsVerificationRepoTests
{
    private const int TestSeed = 404;
    private readonly List<Country> _testCountries;
    private readonly List<Person> _testPeople;
    private readonly List<User> _testUsers;
    private readonly List<DocsVerification> _testDocsVerifications;

    public DocsVerificationRepoTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(10, _testCountries, seed: TestSeed);
        _testUsers = UserFactory.CreateTestUsers(5, _testPeople, seed: TestSeed);
        _testDocsVerifications = DocsVerificationFactory.CreateTestDocsVerifications(
            5, _testPeople, _testUsers, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(
            _testCountries, _testPeople, _testUsers, _testDocsVerifications);
    
    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();


    [Fact]
    public async Task GetListAsync_ShouldReturnAllVerificationsWithPeople()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new DocsVerificationRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testDocsVerifications.Count);
        result.Should().BeEquivalentTo(_testDocsVerifications, options =>
            options.Excluding(d => d.VerifiedByUser)
                .Including(d => d.Person)
                .Excluding(d => d.Person.Country));
    }

    [Fact]
    public async Task GetListAsync_WhenEmpty_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new DocsVerificationRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnVerificationWithPerson()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new DocsVerificationRepository(context);
        var expected = _testDocsVerifications.First();

        // Act
        var result = await repo.GetByIdAsync(expected.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected, options =>
            options.Excluding(d => d.VerifiedByUser)
                .Including(d => d.Person)
                .Excluding(d => d.Person.Country));
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new DocsVerificationRepository(context);
        const int nonExistentId = -1;

        // Act
        var result = await repo.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenValid_ShouldAddVerification()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new DocsVerificationRepository(context);
        var person = _testPeople.First(p => 
            !_testDocsVerifications.Any(d => d.PersonId == p.Id));
        var user = _testUsers.First();

        var newVerification = new DocsVerification
        {
            SubmissionDate = DateTime.UtcNow,
            Status = VerificationStatus.Pending,
            PaidFees = 100,
            PersonId = person.Id,
            Person = null!,
            VerifiedByUserId = user.Id,
            VerifiedByUser = null!
        };

        // Act
        await repo.AddAsync(newVerification);
        var result = await context.DocsVerifications
            .FirstOrDefaultAsync(d => d.PersonId == person.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newVerification, options => 
            options.Excluding(d => d.Id)
                  .Excluding(d => d.Person)
                  .Excluding(d => d.VerifiedByUser));
    }

    [Fact]
    public async Task UpdateAsync_WhenValid_ShouldUpdateVerification()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new DocsVerificationRepository(context);
        var verification = _testDocsVerifications.First();
        var originalStatus = verification.Status;
        verification.Status = VerificationStatus.Approved;
        verification.IsApproved = true;
        verification.VerificationDate = DateTime.UtcNow;

        // Act
        await repo.UpdateAsync(verification);
        var updated = await repo.GetByIdAsync(verification.Id);

        // Assert
        updated.Should().NotBeNull();
        updated!.Status.Should().NotBe(originalStatus);
        updated.Status.Should().Be(VerificationStatus.Approved);
        updated.IsApproved.Should().BeTrue();
        updated.VerificationDate.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenExists_ShouldRemoveVerification()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new DocsVerificationRepository(context);
        var verification = _testDocsVerifications.First();

        // Act
        var result = await repo.DeleteAsync(verification.Id);
        var deleted = await repo.GetByIdAsync(verification.Id);

        // Assert
        result.Should().BeTrue();
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task GetByPersonIdAsync_WhenExists_ShouldReturnVerification()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new DocsVerificationRepository(context);
        var expected = _testDocsVerifications.First();

        // Act
        var result = await repo.GetByPersonIdAsync(expected.PersonId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected, options => 
            options.Excluding(d => d.VerifiedByUser)
                  .Excluding(d => d.Person));
    }

    [Fact]
    public async Task GetByPersonIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new DocsVerificationRepository(context);
        var personWithoutVerification = _testPeople.First(p => 
            !_testDocsVerifications.Any(d => d.PersonId == p.Id));

        // Act
        var result = await repo.GetByPersonIdAsync(personWithoutVerification.Id);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(VerificationStatus.Pending)]
    [InlineData(VerificationStatus.UnderReview)]
    public async Task UpdateStatus_ToApproved_ShouldSetApprovalFields(VerificationStatus fromStatus)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new DocsVerificationRepository(context);
        var verification = _testDocsVerifications.First();
        verification.Status = fromStatus;
        verification.IsApproved = null;
        verification.VerificationDate = null;
        await context.SaveChangesAsync();

        // Act
        verification.Status = VerificationStatus.Approved;
        verification.IsApproved = true;
        verification.VerificationDate = DateTime.UtcNow;
        await repo.UpdateAsync(verification);
        var updated = await repo.GetByIdAsync(verification.Id);

        // Assert
        updated.Should().NotBeNull();
        updated!.Status.Should().Be(VerificationStatus.Approved);
        
        if(verification.Status == VerificationStatus.Approved)
            updated.IsApproved.Should().BeTrue();
        else
            updated.IsApproved.Should().BeNull();
        
        updated.VerificationDate.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateStatus_ToRejected_ShouldSetRejectionFields()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new DocsVerificationRepository(context);
        var verification = _testDocsVerifications.First();
        verification.Status = VerificationStatus.Pending;
        verification.IsApproved = null;
        verification.RejectedReason = null;
        await context.SaveChangesAsync();

        // Act
        verification.Status = VerificationStatus.Rejected;
        verification.IsApproved = false;
        verification.RejectedReason = "Missing documents";
        await repo.UpdateAsync(verification);
        var updated = await repo.GetByIdAsync(verification.Id);

        // Assert
        updated.Should().NotBeNull();
        updated!.Status.Should().Be(VerificationStatus.Rejected);
        updated.IsApproved.Should().BeFalse();
        updated.RejectedReason.Should().NotBeNullOrEmpty();
    }
}