using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class RegistrationRepoTests
{
    private const int TestSeed = 808;
    private readonly List<Country> _testCountries;
    private readonly List<Person> _testPeople;
    private readonly List<Student> _testStudents;
    private readonly List<Program> _testPrograms;
    private readonly List<Semester> _testSemesters;
    private readonly List<Course> _testCourses;
    private readonly List<Section> _testSections;
    private readonly List<User> _testUsers;
    private readonly List<Registration> _testRegistrations;

    public RegistrationRepoTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(10, _testCountries, seed: TestSeed);
        _testStudents = StudentFactory.CreateTestStudents(5, _testPeople, seed: TestSeed);
        _testPrograms = ProgramFactory.CreateTestPrograms(3, seed: TestSeed);
        _testSemesters = SemesterFactory.CreateTestSemesters(5, seed: TestSeed);
        _testCourses = CourseFactory.CreateTestCourses(7, seed: TestSeed);
        _testSections = SectionFactory.CreateTestSections(5, _testCourses, _testSemesters, [], seed: TestSeed);
        _testUsers = UserFactory.CreateTestUsers(3, _testPeople, seed: TestSeed);
        _testRegistrations = RegistrationFactory.CreateTestRegistrations(
            5, _testStudents, _testSections, _testSemesters, _testUsers, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(
            _testCountries, _testPeople, _testStudents,
            _testPrograms, _testSemesters, _testCourses,
            _testSections, _testUsers, _testRegistrations);
    
    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();

    [Fact]
    public async Task GetListAsync_WhenRegistrationsExist_ShouldReturnAllRegistrations()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);

        var result = await repo.GetListAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(_testRegistrations.Count);
        result.Should().BeEquivalentTo(_testRegistrations, options => 
            options.Excluding(r => r.Student)
                  .Excluding(r => r.Section)
                  .Excluding(r => r.Semester)
                  .Excluding(r => r.ProcessedByUser));
    }

    [Fact]
    public async Task GetListAsync_WhenNoRegistrationsExist_ShouldReturnEmptyList()
    {
        await using var context = await GetEmptyDbContext();
        var repo = new RegistrationRepository(context);

        var result = await repo.GetListAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenRegistrationExists_ShouldReturnWithAllIncludes()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);
        var expectedRegistration = _testRegistrations.First();

        var result = await repo.GetByIdAsync(expectedRegistration.Id);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedRegistration, options => 
            options.Excluding(r => r.Student.Person)
            .Excluding(r => r.ProcessedByUser)
            .Excluding(r => r.Section.Course)
            .Excluding(r => r.Section.Semester));
        result!.Student.Should().NotBeNull();
        result.Section.Should().NotBeNull();
        result.Semester.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WhenRegistrationDoesNotExist_ShouldReturnNull()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);
        const int nonExistentId = -1;

        var result = await repo.GetByIdAsync(nonExistentId);

        result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByIdAsync_WhenIdIsInvalid_ShouldReturnNull(int id)
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);

        var result = await repo.GetByIdAsync(id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenRegistrationIsValid_ShouldAddAndSaveRegistration()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);
        var student = _testStudents.Last();
        var section = _testSections.Last();
        var semester = _testSemesters.Last();
        var user = _testUsers.First();

        var newRegistration = new Registration
        {
            RegistrationDate = DateTime.UtcNow,
            RegistrationFees = 250.50m,
            StudentId = student.Id,
            SectionId = section.Id,
            SemesterId = semester.Id,
            ProcessedByUserId = user.Id,
            Student = null!,
            Section = null!,
            Semester = null!
        };

        await repo.AddAsync(newRegistration);
        var result = await context.Registrations
            .FirstOrDefaultAsync(r => r.StudentId == student.Id && 
                                   r.SectionId == section.Id && 
                                   r.SemesterId == semester.Id);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newRegistration, options => 
            options.Excluding(r => r.Id)
                  .Excluding(r => r.Student)
                  .Excluding(r => r.Section)
                  .Excluding(r => r.Semester)
                  .Excluding(r => r.ProcessedByUser));
    }

    [Fact]
    public async Task AddAsync_WhenRegistrationIsNull_ShouldThrowArgumentNullException()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);

        Func<Task> act = async () => await repo.AddAsync(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Fact]
    public async Task UpdateAsync_WhenRegistrationExists_ShouldUpdateAndSaveChanges()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);
        var registrationToUpdate = await context.Registrations.FirstAsync();
        var originalFees = registrationToUpdate.RegistrationFees;
        registrationToUpdate.RegistrationFees += 50;

        await repo.UpdateAsync(registrationToUpdate);
        var result = await context.Registrations.FindAsync(registrationToUpdate.Id);

        result.Should().NotBeNull();
        result!.RegistrationFees.Should().NotBe(originalFees);
        result.RegistrationFees.Should().Be(registrationToUpdate.RegistrationFees);
    }

    [Fact]
    public async Task UpdateAsync_WhenRegistrationDoesNotExist_ShouldThrowException()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);
        var nonExistentRegistration = new Registration
        {
            Id = -1,
            Student = null!,
            Section = null!,
            Semester = null!
        };

        Func<Task> act = async () => await repo.UpdateAsync(nonExistentRegistration);

        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenRegistrationExists_ShouldDeleteAndReturnTrue()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);
        var registrationToDelete = _testRegistrations.First();

        var result = await repo.DeleteAsync(registrationToDelete.Id);
        var deletedRegistration = await repo.GetByIdAsync(registrationToDelete.Id);

        result.Should().BeTrue();
        deletedRegistration.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenRegistrationDoesNotExist_ShouldReturnFalse()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);
        const int nonExistentId = -1;

        var result = await repo.DeleteAsync(nonExistentId);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DoesExistAsync_WhenRegistrationExists_ShouldReturnTrue()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);
        var existingRegistration = _testRegistrations.First();

        var result = await repo.DoesExistAsync(
            existingRegistration.StudentId,
            existingRegistration.SectionId,
            existingRegistration.SemesterId);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesExistAsync_WhenRegistrationDoesNotExist_ShouldReturnFalse()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);
        var student = _testStudents.Last();
        var section = _testSections.Last();
        var semester = _testSemesters.Last();

        // Ensure this combination doesn't exist
        context.Registrations.RemoveRange(
            context.Registrations.Where(r => 
                r.StudentId == student.Id && 
                r.SectionId == section.Id && 
                r.SemesterId == semester.Id));
        await context.SaveChangesAsync();

        var result = await repo.DoesExistAsync(student.Id, section.Id, semester.Id);

        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(1, 0, 1)]
    [InlineData(1, 1, 0)]
    [InlineData(-1, 1, 1)]
    [InlineData(1, -1, 1)]
    [InlineData(1, 1, -1)]
    public async Task DoesExistAsync_WhenAnyIdIsInvalid_ShouldReturnFalse(int studentId, int sectionId, int semesterId)
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);

        var result = await repo.DoesExistAsync(studentId, sectionId, semesterId);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DoesExistAsync_WhenProcessedByUserIsNull_ShouldStillReturnTrue()
    {
        await using var context = await GetDbContext();
        var repo = new RegistrationRepository(context);
        var registrationWithoutProcessor = new Registration
        {
            RegistrationDate = DateTime.UtcNow,
            RegistrationFees = 200.00m,
            StudentId = _testStudents.Last().Id,
            Student = null!,
            SectionId = _testSections.Last().Id,
            Section = null!,
            SemesterId = _testSemesters.Last().Id,
            Semester = null!,
            ProcessedByUserId = null
        };
        await context.Registrations.AddAsync(registrationWithoutProcessor);
        await context.SaveChangesAsync();

        var result = await repo.DoesExistAsync(
            registrationWithoutProcessor.StudentId,
            registrationWithoutProcessor.SectionId,
            registrationWithoutProcessor.SemesterId);

        result.Should().BeTrue();
    }
}