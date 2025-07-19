using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;
using Bogus;
using Person = Domain.Entities.Person;

namespace UnitTests.Infrastructure.Tests;

public class ProfessorRepositoryTests
{
    private const int TestSeed = 101;
    private readonly List<Country> _testCountries;
    private readonly List<Person> _testPeople;
    private readonly List<Professor> _testProfessors;

    public ProfessorRepositoryTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(8, _testCountries, seed: TestSeed);
        _testProfessors = ProfessorFactory.CreateTestProfessors(5, _testPeople, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testCountries, _testPeople, _testProfessors);
    
    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();

    [Fact]
    public async Task GetListAsync_WhenProfessorsExist_ShouldReturnAllProfessors()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testProfessors.Count);
        result.Should().BeEquivalentTo(_testProfessors, options => options.Excluding(p => p.Person));
    }

    [Fact]
    public async Task GetListAsync_WhenNoProfessorsExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new ProfessorRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenProfessorExists_ShouldReturnCorrectProfessor()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);
        var expectedProfessor = _testProfessors.First();

        // Act
        var result = await repo.GetByIdAsync(expectedProfessor.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedProfessor, options => options.Excluding(p => p.Person));
    }

    [Fact]
    public async Task GetByIdAsync_WhenProfessorDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);
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
        var repo = new ProfessorRepository(context);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByEmployeeNumberAsync_WhenProfessorExists_ShouldReturnCorrectProfessor()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);
        var expectedProfessor = _testProfessors.First();

        // Act
        var result = await repo.GetByEmployeeNumberAsync(expectedProfessor.EmployeeNumber);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedProfessor, options => options.Excluding(p => p.Person));
    }

    [Fact]
    public async Task GetByEmployeeNumberAsync_WhenProfessorDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);
        const string nonExistentEmployeeNumber = "nonexistent";

        // Act
        var result = await repo.GetByEmployeeNumberAsync(nonExistentEmployeeNumber);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetByEmployeeNumberAsync_WhenEmployeeNumberIsInvalid_ShouldReturnNull(string employeeNumber)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);

        // Act
        var result = await repo.GetByEmployeeNumberAsync(employeeNumber);

        // Assert
        result.Should().BeNull();
    }
    
    [Fact]
    public async Task AddAsync_WhenProfessorIsValid_ShouldAddAndSaveProfessor()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);
        var person = _testPeople.First(p => _testProfessors.All(prof => prof.PersonId != p.Id));
        var newProfessor = new Faker<Professor>()
            .RuleFor(p => p.EmployeeNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(p => p.AcademicRank, f => f.PickRandom<Domain.Enums.AcademicRank>())
            .RuleFor(p => p.HireDate, f => f.Date.Past(5))
            .RuleFor(p => p.Specialization, f => f.Name.JobTitle())
            .RuleFor(p => p.OfficeLocation, f => f.Address.SecondaryAddress())
            .RuleFor(p => p.Salary, f => f.Finance.Amount(50000, 150000))
            .RuleFor(p => p.IsActive, f => true)
            .RuleFor(p => p.PersonId, person.Id)
            .UseSeed(456)
            .Generate();

        // Act
        await repo.AddAsync(newProfessor);
        var result = await context.Professors
            .FirstOrDefaultAsync(p => p.EmployeeNumber == newProfessor.EmployeeNumber);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newProfessor, 
            options => options.Excluding(x => x.Person));
    }

    [Fact]
    public async Task AddAsync_WhenProfessorIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);

        // Act
        var act = async () => await repo.AddAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_WhenProfessorExists_ShouldUpdateAndSaveChanges()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);
        var professorToUpdate = await context.Professors.FirstAsync();
        var originalSalary = professorToUpdate.Salary;
        professorToUpdate.Salary += 1000;

        // Act
        await repo.UpdateAsync(professorToUpdate);
        var result = await context.Professors.FindAsync(professorToUpdate.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Salary.Should().NotBe(originalSalary);
        result.Salary.Should().Be(professorToUpdate.Salary);
    }

    [Fact]
    public async Task UpdateAsync_WhenProfessorIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);

        // Act
        var act = async () => await repo.UpdateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenProfessorExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);
        var professorToDelete = _testProfessors.First();

        // Act
        var result = await repo.DeleteAsync(professorToDelete.EmployeeNumber);
        var deletedProfessor = await repo.GetByEmployeeNumberAsync(professorToDelete.EmployeeNumber);

        // Assert
        result.Should().BeTrue();
        deletedProfessor.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenProfessorDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);
        const string nonExistentEmployeeNumber = "nonexistent";

        // Act
        var result = await repo.DeleteAsync(nonExistentEmployeeNumber);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task DeleteAsync_WhenEmployeeNumberIsInvalid_ShouldReturnFalse(string employeeNumber)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);

        // Act
        var result = await repo.DeleteAsync(employeeNumber);

        // Assert
        result.Should().BeFalse();
    }



    [Fact]
    public async Task DoesExistAsync_WhenProfessorExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);
        var personId = _testProfessors.First().PersonId;

        // Act
        var result = await repo.DoesExistAsync(personId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesExistAsync_WhenProfessorDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);
        const int nonExistentPersonId = -1;

        // Act
        var result = await repo.DoesExistAsync(nonExistentPersonId);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DoesExistAsync_WhenPersonIdIsInvalid_ShouldReturnFalse(int personId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new ProfessorRepository(context);

        // Act
        var result = await repo.DoesExistAsync(personId);

        // Assert
        result.Should().BeFalse();
    }
}
