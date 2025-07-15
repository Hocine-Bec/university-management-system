using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;
using Xunit.Abstractions;
using Person = Domain.Entities.Person;

namespace UnitTests.Infrastructure.Tests;

public class PersonRepositoryTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private const int TestSeed = 123;
    private readonly List<Country> _testCountries;
    private readonly List<Person> _testPeople;

    public PersonRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(5, _testCountries, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testCountries, _testPeople);

    // GetListAsync Tests
    [Fact]
    public async Task GetListAsync_WhenPeopleExist_ShouldReturnAllPeople()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
        result.Should().BeEquivalentTo(_testPeople);
    }

    [Fact]
    public async Task GetListAsync_WhenNoPeopleExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await InMemoryDbFactory.CreateAsync(); // Empty DB
        var repo = new PersonRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    // GetByIdAsync Tests
    [Fact]
    public async Task GetByIdAsync_WhenPersonExists_ShouldReturnCorrectPerson()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);
        var expectedPerson = _testPeople.First();

        // Act
        var result = await repo.GetByIdAsync(expectedPerson.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedPerson);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPersonNotFound_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);
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
        var repo = new PersonRepository(context);

        // Act
        var result = await repo.GetByIdAsync(invalidId);

        // Assert
        result.Should().BeNull();
    }

    // GetByNameAsync Tests
    [Fact]
    public async Task GetByNameAsync_WhenPersonExists_ShouldReturnCorrectPerson()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);
        var expectedPerson = _testPeople.First(x => x.Id == 1);

        // Act
        var result = await repo.GetByNameAsync(expectedPerson.LastName);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedPerson);
    }

    [Fact]
    public async Task GetByNameAsync_WhenCaseMismatch_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);
        var expectedPerson = _testPeople.First();

        // Act
        var result = await repo.GetByNameAsync(expectedPerson.LastName.ToUpper());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByNameAsync_WhenPersonNotFound_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);
        const string nonExistentName = "NonExistentName";

        // Act
        var result = await repo.GetByNameAsync(nonExistentName);

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
        var repo = new PersonRepository(context);

        // Act
        var result = await repo.GetByNameAsync(invalidName);

        // Assert
        result.Should().BeNull();
    }

    // AddAsync Tests
    [Fact]
    public async Task AddAsync_WhenPersonIsValid_ShouldAddAndSavePerson()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);

        var newPerson = new Faker<Person>()
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.DOB, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
            .RuleFor(p => p.Address, f => f.Address.StreetAddress())
            .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(p => p.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(p => p.CountryId, _testCountries.First().Id)
            .UseSeed(456)
            .Generate();

        // Act
        var newId = await repo.AddAsync(newPerson);
        var result = await repo.GetByIdAsync(newId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newPerson);
        result.FirstName.Should().Be(newPerson.FirstName);
        result.LastName.Should().Be(newPerson.LastName);
    }

    [Fact]
    public async Task AddAsync_WhenPersonIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);

        // Act
        var act = async () => await repo.AddAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    // UpdateAsync Tests
    [Fact]
    public async Task UpdateAsync_WhenPersonExists_ShouldUpdateAndSaveChanges()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);
        var personToUpdate = await context.People.FirstAsync();
        personToUpdate.FirstName = "UpdatedName";

        // Act
        await repo.UpdateAsync(personToUpdate);
        var result = await context.People.FindAsync(personToUpdate.Id);

        // Assert
        result.Should().NotBeNull();
        result!.FirstName.Should().Be("UpdatedName");
    }

    [Fact]
    public async Task UpdateAsync_WhenPersonIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);

        // Act
        var act = async () => await repo.UpdateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    // DoesExistAsync Tests
    [Fact]
    public async Task DoesExistAsync_WhenPersonExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);
        var person = _testPeople.First();

        // Act
        var result = await repo.DoesExistAsync(person.LastName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesExistAsync_WhenPersonDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);
        const string nonExistentName = "NonExistentName";

        // Act
        var result = await repo.DoesExistAsync(nonExistentName);

        // Assert
        result.Should().BeFalse();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task DoesExistAsync_WhenNameIsInvalid_ShouldReturnFalse(string invalidName)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);

        // Act
        var result = await repo.DoesExistAsync(invalidName);

        // Assert
        result.Should().BeFalse();
    }

    // DeleteAsync Tests
    [Fact]
    public async Task DeleteAsync_WhenPersonExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);
        var personToDelete = _testPeople.First(x => x.Id == 3);

        // Act
        var result = await repo.DeleteAsync(personToDelete.LastName);
        var deletedPerson = await repo.GetByNameAsync(personToDelete.LastName);
        
        // Assert
        result.Should().BeTrue();
        deletedPerson.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenPersonNotFound_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);
        const string nonExistentName = "NonExistentName";

        // Act
        var result = await repo.DeleteAsync(nonExistentName);

        // Assert
        result.Should().BeFalse();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task DeleteAsync_WhenNameIsInvalid_ShouldReturnFalse(string invalidName)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PersonRepository(context);

        // Act
        var result = await repo.DeleteAsync(invalidName);

        // Assert
        result.Should().BeFalse();
    }
}