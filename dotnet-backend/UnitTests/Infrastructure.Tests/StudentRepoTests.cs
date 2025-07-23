using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;
using Xunit.Abstractions;
using Person = Domain.Entities.Person;

namespace UnitTests.Infrastructure.Tests;

public class StudentRepoTests
{
    private const int TestSeed = 456;
    private readonly List<Country> _testCountries;
    private readonly List<Person> _testPeople;
    private readonly List<Student> _testStudents;

    public StudentRepoTests(ITestOutputHelper testOutputHelper)
    {
        _testCountries = CountryFactory.CreateTestCountries(10, seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(8, _testCountries, seed: TestSeed);
        _testStudents = StudentFactory.CreateTestStudents(5, _testPeople, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testCountries, _testPeople, _testStudents);
    
    private static async Task<AppDbContext> GetEmptyDbContext() => await InMemoryDbFactory.CreateAsync();

    [Fact]
    public async Task GetListAsync_WhenStudentsExist_ShouldReturnAllStudentsWithPeople()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
        result.Should().BeEquivalentTo(_testStudents, 
            options => options.Excluding(s => s.Person.Country));
    }
    
    [Fact]
    public async Task GetListAsync_WhenNoStudentsExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new StudentRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenStudentExists_ShouldReturnCorrectStudentWithPerson()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        var expectedStudent = _testStudents.First();

        // Act
        var result = await repo.GetByIdAsync(expectedStudent.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedStudent, 
            options => options.Excluding(s => s.Person.Country));
        result.Person.Should().NotBeNull();
        result.Person.CountryId.Should().Be(expectedStudent.Person.CountryId);
    }
    
    [Fact]
    public async Task GetByIdAsync_WhenStudentDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        const int nonExistentId = -1;

        // Act
        var result = await repo.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByIdAsync_WhenIdIsInvalid_ShouldReturnNull(int invalid)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);

        // Act
        var result = await repo.GetByIdAsync(invalid);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByStudentNumberAsync_WhenStudentExists_ShouldReturnCorrectStudent()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        var expectedStudent = _testStudents.First();

        // Act
        var result = await repo.GetByStudentNumberAsync(expectedStudent.StudentNumber);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedStudent, options => 
            options.Excluding(s => s.Person.Country));
        result.Person.Should().NotBeNull();
        result.Person.CountryId.Should().Be(expectedStudent.Person.CountryId);
    }
    
    [Fact]
    public async Task GetByStudentNumberAsync_WhenStudentDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        const string nonExistentStudentNumber = "non-existent";

        // Act
        var result = await repo.GetByStudentNumberAsync(nonExistentStudentNumber);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetByStudentNumberAsync_WhenStudentNumberIsInvalid_ShouldReturnNull(string studentNumber)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);

        // Act
        var result = await repo.GetByStudentNumberAsync(studentNumber);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenStudentIsValid_ShouldAddAndReturnId()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        var person = _testPeople.Last();
    
        
        var newStudent = new Faker<Student>()
            .RuleFor(s => s.StudentNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(s => s.StudentStatus, f => f.PickRandom<StudentStatus>())
            .RuleFor(s => s.PersonId, person.Id)
            .UseSeed(456)
            .Generate();
        
        // Act
        var newId = await repo.AddAsync(newStudent);
        var result = await context.Students.FindAsync(newId);
        
        // Assert
        newId.Should().BeGreaterThan(0);
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newStudent);
    }
    
    [Fact]
    public async Task AddAsync_WhenStudentIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);

        // Act
        var act = async () => await repo.AddAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_WhenStudentExists_ShouldUpdateAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        var studentToUpdate = await context.Students.FirstAsync();
        studentToUpdate.Notes = "Updated Note";

        // Act
        var result = await repo.UpdateAsync(studentToUpdate);
        var updatedStudent = await context.Students.FindAsync(studentToUpdate.Id);

        // Assert
        result.Should().BeTrue();
        updatedStudent.Should().NotBeNull();
        updatedStudent.Notes.Should().Be("Updated Note");
    }
    
    [Fact]
    public async Task UpdateAsync_WhenStudentIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);

        // Act
        var act = async () => await repo.UpdateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenStudentExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        var studentToDelete = _testStudents.First();

        // Act
        var result = await repo.DeleteAsync(studentToDelete.StudentNumber);
        var deletedStudent = await repo.GetByStudentNumberAsync(studentToDelete.StudentNumber);

        // Assert
        result.Should().BeTrue();
        deletedStudent.Should().BeNull();
    }
    
    [Fact]
    public async Task DeleteAsync_WhenStudentDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        const string nonExistentStudentNumber = "non-existent";

        // Act
        var result = await repo.DeleteAsync(nonExistentStudentNumber);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task DeleteAsync_WhenStudentNumberIsInvalid_ShouldReturnFalse(string studentNumber)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);

        // Act
        var result = await repo.DeleteAsync(studentNumber);

        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task DoesExistAsync_WhenStudentExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        var personId = _testStudents.First().PersonId;

        // Act
        var result = await repo.DoesExistAsync(personId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesExistAsync_WhenStudentDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        const int nonExistentPersonId = -1;

        // Act
        var result = await repo.DoesExistAsync(nonExistentPersonId);

        // Assert
        result.Should().BeFalse();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DoesExistAsync_WhenPersonIdIsInvalid_ShouldReturnFalse(int invalidPersonId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);

        // Act
        var result = await repo.DoesExistAsync(invalidPersonId);

        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task DoesExistsAsync_WhenStudentExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        var studentId = _testStudents.First().Id;

        // Act
        var result = await repo.DoesExistsAsync(studentId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesExistsAsync_WhenStudentDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);
        const int nonExistentId = -1;

        // Act
        var result = await repo.DoesExistsAsync(nonExistentId);

        // Assert
        result.Should().BeFalse();
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DoesExistsAsync_WhenStudentIdIsInvalid_ShouldReturnFalse(int invalidId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new StudentRepository(context);

        // Act
        var result = await repo.DoesExistsAsync(invalidId);

        // Assert
        result.Should().BeFalse();
    }
}