using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class CountryRepositoryTests
{
    private const int TestSeed = 123;
    private readonly List<Country> _testCountries;

    public CountryRepositoryTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testCountries);

    // GetListAsync Tests
    [Fact]
    public async Task GetListAsync_WhenCountriesExist_ShouldReturnAllCountries()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CountryRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
        result.Should().BeEquivalentTo(_testCountries);
    }

    [Fact]
    public async Task GetListAsync_WhenNoCountriesExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await InMemoryDbFactory.CreateAsync(); // Empty DB
        var repo = new CountryRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    // GetByIdAsync Tests
    [Fact]
    public async Task GetByIdAsync_WhenCountryExists_ShouldReturnCorrectCountry()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CountryRepository(context);
        var expectedCountry = _testCountries.First(x => x.Id == 2);

        // Act
        var result = await repo.GetByIdAsync(expectedCountry.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedCountry);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCountryNotFound_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CountryRepository(context);
        const int nonExistentId = -1;

        // Act
        var result = await repo.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByIdAsync_WhenIdIsInvalid_ShouldReturnNull(int invalidId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CountryRepository(context);

        // Act
        var result = await repo.GetByIdAsync(invalidId);

        // Assert
        result.Should().BeNull();
    }

    // GetByCodeAsync Tests
    [Fact]
    public async Task GetByCodeAsync_WhenCountryExists_ShouldReturnCorrectCountry()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CountryRepository(context);
        var code = _testCountries.First(x => x.Id == 2).Code;
        var expectedCountry = _testCountries.First(x => x.Id == 2);

        // Act
        var result = await repo.GetByCodeAsync(code);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedCountry);
    }

    [Fact]
    public async Task GetByCodeAsync_WhenCountryNotFound_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CountryRepository(context);
        const string nonExistentCode = "ZZ";

        // Act
        var result = await repo.GetByCodeAsync(nonExistentCode);

        // Assert
        result.Should().BeNull();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetByCodeAsync_WhenCodeIsInvalid_ShouldReturnNull(string invalidCode)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CountryRepository(context);

        // Act
        var result = await repo.GetByCodeAsync(invalidCode);

        // Assert
        result.Should().BeNull();
    }

    // GetByNameAsync Tests
    [Fact]
    public async Task GetByNameAsync_WhenCountryExists_ShouldReturnCorrectCountry()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CountryRepository(context);
        var name = _testCountries.First().Name;
        var expectedCountry = _testCountries.First();

        // Act
        var result = await repo.GetByNameAsync(name);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedCountry);
    }

    [Fact]
    public async Task GetByNameAsync_WhenCountryNotFound_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CountryRepository(context);
        const string nonExistentName = "Nonexistent Country";

        // Act
        var result = await repo.GetByNameAsync(nonExistentName);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByNameAsync_WhenCaseMismatch_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CountryRepository(context);
        var name = _testCountries.First().Name.ToLower();

        // Act
        var result = await repo.GetByNameAsync(name);

        // Assert
        result.Should().BeNull();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetByNameAsync_WhenNameIsInvalid_ShouldReturnNull(string invalidName)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CountryRepository(context);

        // Act
        var result = await repo.GetByNameAsync(invalidName);

        // Assert
        result.Should().BeNull();
    }
}