using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class CourseRepoTests
{
    private const int TestSeed = 126;
    private readonly List<Course> _testCourses = CourseFactory.CreateTestCourses(seed: TestSeed);

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testCourses);

    [Fact]
    public async Task GetListAsync_WhenEntitiesExist_ShouldReturnAllEntities()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
        result.Should().BeEquivalentTo(_testCourses);
    }

    [Fact]
    public async Task GetListAsync_WhenNoEntitiesExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await InMemoryDbFactory.CreateAsync();
        var repo = new CourseRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityExists_ShouldReturnCorrectEntity()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        var course = _testCourses.First();
        var courseId = course.Id;

        // Act
        var result = await repo.GetByIdAsync(courseId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(course);
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityNotFound_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        const int courseId = 999;

        // Act
        var result = await repo.GetByIdAsync(courseId);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetByIdAsync_WhenIdIsInvalid_ShouldReturnNull(int courseId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);

        // Act
        var result = await repo.GetByIdAsync(courseId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByCodeAsync_WhenEntityExists_ShouldReturnCorrectEntity()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        var course = _testCourses.First();
        var courseCode = course.Code;

        // Act
        var result = await repo.GetByCodeAsync(courseCode);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(course);
    }

    [Fact]
    public async Task GetByCodeAsync_WhenEntityNotFound_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        const string courseCode = "C999";

        // Act
        var result = await repo.GetByCodeAsync(courseCode);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetByCodeAsync_WhenCodeIsInvalid_ShouldReturnNull(string courseCode)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);

        // Act
        var result = await repo.GetByCodeAsync(courseCode);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByCodeAsync_WhenCaseMismatch_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        var course = _testCourses.First();
        var courseCode = course.Code.ToLower(); // Case mismatch

        // Act
        var result = await repo.GetByCodeAsync(courseCode);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenEntityIsValid_ShouldAddAndSaveEntity()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        var newCourse = new Faker<Course>()
            .RuleFor(c => c.Code, f => $"C{f.Random.Number(100, 999)}")
            .RuleFor(c => c.Title, f => f.Lorem.Sentence(3))
            .RuleFor(c => c.Description, f => f.Lorem.Paragraph())
            .RuleFor(c => c.CreditHours, f => f.Random.Int(1, 4))
            .RuleFor(c => c.IsActive, f => f.Random.Bool())
            .UseSeed(456)
            .Generate();
        
        // Act
        await repo.AddAsync(newCourse);
        var result = await context.Courses.SingleOrDefaultAsync(c => c.Code == newCourse.Code);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newCourse);
    }

    [Fact]
    public async Task AddAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);

        // Act
        var act = async () => await repo.AddAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_WhenEntityExists_ShouldUpdateAndSaveChanges()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        var courseToUpdate = await context.Courses.FirstAsync();
        courseToUpdate.Title = "Updated Course Title";

        // Act
        await repo.UpdateAsync(courseToUpdate);
        var result = await context.Courses.FirstAsync(c => c.Id == courseToUpdate.Id);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Updated Course Title");
    }

    [Fact]
    public async Task UpdateAsync_WhenEntityIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);

        // Act
        var act = async () => await repo.UpdateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DoesExistsAsync_WhenEntityExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        var course = _testCourses.First();
        var courseId = course.Id;

        // Act
        var result = await repo.DoesExistsAsync(courseId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesExistsAsync_WhenEntityDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        const int courseId = -1;

        // Act
        var result = await repo.DoesExistsAsync(courseId);


        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task DoesCodeExistAsync_WhenCodeIsValid_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        var course = _testCourses.First();
        var courseCode = course.Code;
        
        // Act
        var result = await repo.DoesCodeExistAsync(courseCode);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task DoesCodeExistAsync_WhenCodeIsInvalid_ShouldReturnFalse(string courseCode)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);

        // Act
        var result = await repo.DoesCodeExistAsync(courseCode);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        var courseToDelete = await context.Courses.FirstAsync();

        // Act
        var deleted = await repo.DeleteAsync(courseToDelete.Id);
        var result = await context.Courses.FirstOrDefaultAsync(c => c.Id == courseToDelete.Id);

        // Assert
        deleted.Should().BeTrue();
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityNotFound_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        const int courseCode = -1;

        // Act
        var result = await repo.DeleteAsync(courseCode);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityWithCodeExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);
        var courseToDelete = await context.Courses.FirstAsync();

        // Act
        var deleted = await repo.DeleteAsync(courseToDelete.Code);
        var result = await context.Courses.FirstOrDefaultAsync(c => c.Code == courseToDelete.Code);

        // Assert
        deleted.Should().BeTrue();
        result.Should().BeNull();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task DeleteAsync_WhenCodeIsInvalid_ShouldReturnFalse(string courseCode)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new CourseRepository(context);

        // Act
        var result = await repo.DeleteAsync(courseCode);

        // Assert
        result.Should().BeFalse();
    }
}
