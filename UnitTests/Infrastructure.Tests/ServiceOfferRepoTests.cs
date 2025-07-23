using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class ServiceOfferRepoTests
{
    private const int TestSeed = 303;
    private readonly List<ServiceOffer> _testServiceOffers = ServiceOfferFactory.CreateTestServiceOffers(5, seed: TestSeed);

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testServiceOffers);
    
    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();

    [Fact]
    public async Task GetListAsync_WhenServiceOffersExist_ShouldReturnAllServiceOffers()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testServiceOffers.Count);
        result.Should().BeEquivalentTo(_testServiceOffers);
    }

    [Fact]
    public async Task GetListAsync_WhenNoServiceOffersExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new ServiceOfferRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenServiceOfferExists_ShouldReturnCorrectServiceOffer()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);
        var expectedServiceOffer = _testServiceOffers.First();

        // Act
        var result = await repo.GetByIdAsync(expectedServiceOffer.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedServiceOffer);
    }

    [Fact]
    public async Task GetByIdAsync_WhenServiceOfferDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);
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
        var repo = new ServiceOfferRepository(context);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenServiceOfferIsValid_ShouldAddAndSaveServiceOffer()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);
        var newServiceOffer = new Faker<ServiceOffer>()
            .RuleFor(s => s.Name, f => f.Commerce.ProductName())
            .RuleFor(s => s.Description, f => f.Lorem.Sentence())
            .RuleFor(s => s.Fees, f => f.Finance.Amount(10, 500))
            .RuleFor(s => s.IsActive, true)
            .UseSeed(404)
            .Generate();

        // Act
        await repo.AddAsync(newServiceOffer);
        var result = await context.ServiceOffers
            .FirstOrDefaultAsync(s => s.Name == newServiceOffer.Name);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newServiceOffer);
    }

    [Fact]
    public async Task AddAsync_WhenServiceOfferIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);

        // Act
        var act = async () => await repo.AddAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_WhenServiceOfferExists_ShouldUpdateAndSaveChanges()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);
        var serviceOfferToUpdate = await context.ServiceOffers.FirstAsync();
        var originalFees = serviceOfferToUpdate.Fees;
        serviceOfferToUpdate.Fees += 10;

        // Act
        await repo.UpdateAsync(serviceOfferToUpdate);
        var result = await context.ServiceOffers.FindAsync(serviceOfferToUpdate.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Fees.Should().NotBe(originalFees);
        result.Fees.Should().Be(serviceOfferToUpdate.Fees);
    }

    [Fact]
    public async Task UpdateAsync_WhenServiceOfferIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);

        // Act
        var act = async () => await repo.UpdateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenServiceOfferExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);
        var serviceOfferToDelete = _testServiceOffers.First();

        // Act
        var result = await repo.DeleteAsync(serviceOfferToDelete.Id);
        var deletedServiceOffer = await repo.GetByIdAsync(serviceOfferToDelete.Id);

        // Assert
        result.Should().BeTrue();
        deletedServiceOffer.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenServiceOfferDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);
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
        var repo = new ServiceOfferRepository(context);

        // Act
        var result = await repo.DeleteAsync(id);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DoesExistAsync_WhenServiceOfferExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);
        var existingName = _testServiceOffers.First().Name;

        // Act
        var result = await repo.DoesExistAsync(existingName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesExistAsync_WhenServiceOfferDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);
        const string nonExistentName = "Nonexistent Service";

        // Act
        var result = await repo.DoesExistAsync(nonExistentName);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task DoesExistAsync_WhenNameIsInvalid_ShouldReturnFalse(string name)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceOfferRepository(context);

        // Act
        var result = await repo.DoesExistAsync(name);

        // Assert
        result.Should().BeFalse();
    }
}