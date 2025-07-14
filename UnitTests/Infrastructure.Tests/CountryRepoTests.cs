using Domain.Entities;
using FluentAssertions;
using Infrastructure.Repositories;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class CountryRepositoryTests
{
    const int TestSeed = 123; // Fixed seed
    private readonly List<Country> _testCountries;

    public CountryRepositoryTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
    }

        
    [Fact]
    public async Task GetListAsync_ShouldReturnAllCountries()
    {
        // Arrange
        using var context = await InMemoryDbFactory.CreateInMemoryDbContext(_testCountries);
        var repo = new CountryRepository(context);
        
        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
        result.Should().BeEquivalentTo(_testCountries);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectCountry()
    {
        // Arrange
        using var context = await InMemoryDbFactory.CreateInMemoryDbContext(_testCountries);
        var repo = new CountryRepository(context);
        var expectedCountry = _testCountries[1]; 

        // Act
        var result = await repo.GetByIdAsync(2);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedCountry);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCountryNotFound()
    {
        // Arrange
        using var context = await InMemoryDbFactory.CreateInMemoryDbContext(_testCountries);
        var repo = new CountryRepository(context);

        // Act
        var result = await repo.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByCodeAsync_ShouldReturnCorrectCountry()
    {
        // Arrange
        using var context = await InMemoryDbFactory.CreateInMemoryDbContext(_testCountries);
        var repo = new CountryRepository(context);
        var code = _testCountries[2].Code;
        var expectedCountry = _testCountries[2]; // Mexico

        // Act
        var result = await repo.GetByCodeAsync(code);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedCountry);
    }

    [Fact]
    public async Task GetByCodeAsync_ShouldReturnNull_WhenCountryNotFound()
    {
        // Arrange
        using var context = await InMemoryDbFactory.CreateInMemoryDbContext(_testCountries);
        var repo = new CountryRepository(context);

        // Act
        var result = await repo.GetByCodeAsync("ZZ");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnCorrectCountry()
    {
        // Arrange
        using var context = await InMemoryDbFactory.CreateInMemoryDbContext(_testCountries);
        var repo = new CountryRepository(context);
        var name = _testCountries[0].Name;
        var expectedCountry = _testCountries[0]; // United States

        // Act
        var result = await repo.GetByNameAsync(name);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedCountry);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenCountryNotFound()
    {
        // Arrange
        using var context = await InMemoryDbFactory.CreateInMemoryDbContext(_testCountries);
        var repo = new CountryRepository(context);

        // Act
        var result = await repo.GetByNameAsync("Nonexistent Country");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByNameAsync_ShouldBeCaseSensitive()
    {
        // Arrange
        using var context = await InMemoryDbFactory.CreateInMemoryDbContext(_testCountries);
        var repo = new CountryRepository(context);
        var name = _testCountries[1].Name.ToLower();

        // Act
        var result = await repo.GetByNameAsync(name);

        // Assert
        result.Should().BeNull();
    }
}