using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class EnrollmentRepoTests
{
    private const int TestSeed = 303;
    private readonly List<Student> _testStudents;
    private readonly List<Program> _testPrograms;
    private readonly List<ServiceApplication> _testApplications;
    private readonly List<Enrollment> _testEnrollments;
    private readonly List<Country> _testCountries;
    private readonly List<Person> _testPeople;
    private readonly List<User> _testUsers;
    private readonly List<ServiceOffer> _testServiceOffers;

    public EnrollmentRepoTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(12, _testCountries, seed: TestSeed);
        _testStudents = StudentFactory.CreateTestStudents(8, _testPeople, seed: TestSeed);
        _testPrograms = ProgramFactory.CreateTestPrograms(10, seed: TestSeed);
        _testUsers = UserFactory.CreateTestUsers(5, _testPeople, seed: TestSeed);
        _testServiceOffers = ServiceOfferFactory.CreateTestServiceOffers(10, seed: TestSeed);
        _testApplications = ServiceApplicationFactory.CreateTestServiceApplications(8, 
            _testPeople, _testServiceOffers, _testUsers, seed: TestSeed);
        _testEnrollments = EnrollmentFactory.CreateTestEnrollments(
            5, _testStudents, _testPrograms, _testApplications, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext()
    {
        return await InMemoryDbFactory.CreateAsync(
            _testCountries, _testPeople, _testStudents, _testPrograms,
            _testServiceOffers, _testUsers, _testApplications, _testEnrollments);
    }
    
    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();


    [Fact]
    public async Task GetListAsync_ShouldReturnAllEnrollmentsWithPrograms()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testEnrollments.Count);
        result.Should().BeEquivalentTo(_testEnrollments, options => 
            options.Excluding(e => e.Student)
                  .Excluding(e => e.ServiceApplication)
                  .Including(e => e.Program));
    }

    [Fact]
    public async Task GetListAsync_WhenNoEnrollments_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new EnrollmentRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEnrollmentWithProgram()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);
        var expectedEnrollment = _testEnrollments.First();

        // Act
        var result = await repo.GetByIdAsync(expectedEnrollment.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedEnrollment, options => 
            options.Excluding(e => e.Student)
                  .Excluding(e => e.ServiceApplication)
                  .Including(e => e.Program));
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);
        const int nonExistentId = -1;

        // Act
        var result = await repo.GetByIdAsync(nonExistentId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByIdAsync_WhenInvalidId_ShouldReturnNull(int invalidId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);

        // Act
        var result = await repo.GetByIdAsync(invalidId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenValid_ShouldAddEnrollment()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);
        
        var availableStudent = _testStudents.First(s => _testEnrollments.All(e => e.StudentId != s.Id));
        var program = _testPrograms.First();
        var serviceApp = ServiceApplicationFactory.CreateTestServiceApplications(1, 
            _testPeople, _testServiceOffers, _testUsers, seed: TestSeed).First();
        serviceApp.Id = _testApplications.Last().Id + 1;
        
        var newEnrollment = new Enrollment
        {
            EnrollmentDate = DateTime.UtcNow,
            Status = EnrollmentStatus.Active,
            StudentId = availableStudent.Id,
            Student = null!,
            ProgramId = program.Id,
            Program = null!,
            ServiceApplicationId = serviceApp.Id,
            ServiceApplication = null!
        };

        // Act
        await repo.AddAsync(newEnrollment);
        var result = await context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == availableStudent.Id && e.ProgramId == program.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newEnrollment, options => 
            options.Excluding(e => e.Id)
                  .Excluding(e => e.Student)
                  .Excluding(e => e.Program)
                  .Excluding(e => e.ServiceApplication));
    }

    [Fact]
    public async Task AddAsync_WhenNull_ShouldThrowException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.AddAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_WhenValid_ShouldUpdateEnrollment()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);
        var enrollment = _testEnrollments.First();
        var originalStatus = enrollment.Status;
        enrollment.Status = EnrollmentStatus.Withdrawn;

        // Act
        await repo.UpdateAsync(enrollment);
        var updated = await repo.GetByIdAsync(enrollment.Id);

        // Assert
        updated.Should().NotBeNull();
        updated!.Status.Should().NotBe(originalStatus);
        updated.Status.Should().Be(EnrollmentStatus.Withdrawn);
    }

    [Fact]
    public async Task UpdateAsync_WhenNull_ShouldThrowException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpdateAsync(null!));
    }

    [Fact]
    public async Task DeleteAsync_WhenExists_ShouldRemoveEnrollment()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);
        var enrollment = _testEnrollments.First();

        // Act
        var result = await repo.DeleteAsync(enrollment.Id);
        var deleted = await repo.GetByIdAsync(enrollment.Id);

        // Assert
        result.Should().BeTrue();
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenNotExists_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);
        const int nonExistentId = -1;

        // Act
        var result = await repo.DeleteAsync(nonExistentId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetByStudentIdAsync_WhenExists_ShouldReturnEnrollment()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);
        var expected = _testEnrollments.First();

        // Act
        var result = await repo.GetByStudentIdAsync(expected.StudentId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected, options => 
            options.Excluding(e => e.Student)
                  .Excluding(e => e.ServiceApplication)
                  .Including(e => e.Program));
    }

    [Fact]
    public async Task GetByStudentIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);
        var studentWithoutEnrollment = _testStudents.First(s => 
            !_testEnrollments.Any(e => e.StudentId == s.Id));

        // Act
        var result = await repo.GetByStudentIdAsync(studentWithoutEnrollment.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CanEnrollInProgramAsync_WhenNoConflict_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);
        var student = _testStudents.First(s => 
            !_testEnrollments.Any(e => e.StudentId == s.Id));
        var program = _testPrograms.First();

        // Act
        var result = await repo.CanEnrollInProgramAsync(student.Id, program.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(EnrollmentStatus.Active)]
    [InlineData(EnrollmentStatus.OnLeave)]
    [InlineData(EnrollmentStatus.Graduated)]
    public async Task CanEnrollInProgramAsync_WhenConflictExists_ShouldReturnFalse(EnrollmentStatus status)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);
        var enrollment = _testEnrollments.First();
        enrollment.Status = status;
        await context.SaveChangesAsync();

        // Act
        var result = await repo.CanEnrollInProgramAsync(enrollment.StudentId, enrollment.ProgramId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CanEnrollInProgramAsync_WhenNonConflictStatus_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new EnrollmentRepository(context);
        var enrollment = _testEnrollments.First();
        enrollment.Status = EnrollmentStatus.Withdrawn;
        await context.SaveChangesAsync();

        // Act
        var result = await repo.CanEnrollInProgramAsync(enrollment.StudentId, enrollment.ProgramId);

        // Assert
        result.Should().BeTrue();
    }

}