using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class ServiceApplicationRepositoryTests
{
    private const int TestSeed = 606;
    private readonly List<Country> _testCountries;
    private readonly List<Person> _testPeople;
    private readonly List<User> _testUsers;
    private readonly List<ServiceOffer> _testServiceOffers;
    private readonly List<ServiceApplication> _testApplications;

    public ServiceApplicationRepositoryTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(10, _testCountries, seed: TestSeed);
        _testUsers = UserFactory.CreateTestUsers(5, _testPeople, seed: TestSeed);
        _testServiceOffers = ServiceOfferFactory.CreateTestServiceOffers(8, seed: TestSeed);
        _testApplications = ServiceApplicationFactory.CreateTestServiceApplications(
            5, _testPeople, _testServiceOffers, _testUsers, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testCountries, _testPeople, _testUsers,
            _testServiceOffers, _testApplications);

    [Fact]
    public async Task GetListAsync_ShouldReturnApplicationsWithIncludes()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceApplicationRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testApplications.Count);
        result.Should().BeEquivalentTo(_testApplications, options => 
            options.Excluding(a => a.Person)
                  .Excluding(a => a.ServiceOffer)
                  .Excluding(a => a.ProcessedByUser));
        
        // Verify includes
        result.First().Person.Should().NotBeNull();
        result.First().ServiceOffer.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnApplicationWithIncludes()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceApplicationRepository(context);
        var expectedApp = _testApplications.First();

        // Act
        var result = await repo.GetByIdAsync(expectedApp.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedApp, options => 
            options.Excluding(a => a.Person)
                  .Excluding(a => a.ServiceOffer)
                  .Excluding(a => a.ProcessedByUser));
        
        // Verify includes
        result!.Person.Should().NotBeNull();
        result.ServiceOffer.Should().NotBeNull();
        if (expectedApp.ProcessedByUserId.HasValue)
        {
            result.ProcessedByUser.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task GetByPersonIdAsync_ShouldReturnApplicationsForPerson()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceApplicationRepository(context);
        var testPerson = _testPeople.First();
        var expectedApps = _testApplications
            .Where(a => a.PersonId == testPerson.Id)
            .ToList();

        // Act
        var result = await repo.GetByPersonIdAsync(testPerson.Id);

        // Assert
        result.Should().NotBeNull();
        if (expectedApps.Count != 0)
        {
            result.Should().BeEquivalentTo(expectedApps.First(), options => 
                options.Excluding(a => a.Person)
                      .Excluding(a => a.ServiceOffer)
                      .Excluding(a => a.ProcessedByUser));
        }
        else
        {
            result.Should().BeNull();
        }
    }

    [Fact]
    public async Task DoesPersonHaveActiveApplicationsAsync_WhenActiveExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceApplicationRepository(context);
        
        // Create an active application
        var activeApp = _testApplications.First(a => 
            a.Status == ApplicationStatus.New || a.Status == ApplicationStatus.InProgress);
        var personId = activeApp.PersonId;
        var serviceId = activeApp.ServiceOfferId;

        // Act
        var result = await repo.DoesPersonHaveActiveApplicationsAsync(personId, serviceId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesPersonHaveActiveApplicationsAsync_WhenNoActiveExists_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceApplicationRepository(context);
        
        // Find a completed application
        var completedApp = _testApplications.FirstOrDefault(a => 
            a.Status == ApplicationStatus.Completed);
        var personId = completedApp?.PersonId ?? _testPeople.Last().Id;
        var serviceId = completedApp?.ServiceOfferId ?? _testServiceOffers.Last().Id;

        // Act
        var result = await repo.DoesPersonHaveActiveApplicationsAsync(personId, serviceId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DoesExistsAsync_WhenApplicationExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceApplicationRepository(context);
        var existingId = _testApplications.First().Id;

        // Act
        var result = await repo.DoesExistsAsync(existingId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AddAsync_WhenApplicationIsValid_ShouldAddAndSave()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceApplicationRepository(context);
        var person = _testPeople.First(p => _testApplications.All(a => a.PersonId != p.Id));
        var service = _testServiceOffers.First();

        var newApp = new ServiceApplication
        {
            ApplicationDate = DateTime.Now,
            Status = ApplicationStatus.New,
            PaidFees = 0,
            PersonId = person.Id,
            Person = null!,
            ServiceOfferId = service.Id,
            ServiceOffer = null!
        };

        // Act
        await repo.AddAsync(newApp);
        var result = await context.ServiceApplications
            .FirstOrDefaultAsync(a => a.PersonId == person.Id && a.ServiceOfferId == service.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(ApplicationStatus.New);
        result.PersonId.Should().Be(person.Id);
        result.ServiceOfferId.Should().Be(service.Id);
    }

    [Fact]
    public async Task UpdateAsync_WhenStatusChanges_ShouldUpdateProperly()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ServiceApplicationRepository(context);
        var appToUpdate = await context.ServiceApplications.FirstAsync();
        var originalStatus = appToUpdate.Status;
        var newStatus = ApplicationStatus.Completed;
        appToUpdate.Status = newStatus;
        appToUpdate.CompletedDate = DateTime.Now;
        appToUpdate.PaidFees = appToUpdate.ServiceOffer.Fees;

        // Act
        await repo.UpdateAsync(appToUpdate);
        var result = await context.ServiceApplications.FindAsync(appToUpdate.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(newStatus);
        result.Status.Should().NotBe(originalStatus);
        result.CompletedDate.Should().NotBeNull();
        result.PaidFees.Should().Be(appToUpdate.ServiceOffer.Fees);
    }
}