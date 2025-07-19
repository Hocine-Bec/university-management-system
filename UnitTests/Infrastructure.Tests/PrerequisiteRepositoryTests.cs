using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class PrerequisiteRepositoryTests
{
    private const int TestSeed = 404;
    private readonly List<Course> _testCourses;
    private readonly List<Prerequisite> _testPrerequisites;

    public PrerequisiteRepositoryTests()
    {
        _testCourses = CourseFactory.CreateTestCourses(5, seed: TestSeed);
        _testPrerequisites = PrerequisiteFactory.CreateTestPrerequisites(3, _testCourses, seed: TestSeed);
    }

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testCourses, _testPrerequisites);
    
    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();

    [Fact]
    public async Task GetListAsync_WhenPrerequisitesExist_ShouldReturnAllPrerequisites()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PrerequisiteRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testPrerequisites.Count);
        result.Should().BeEquivalentTo(_testPrerequisites, 
            options => options.Excluding(p => p.Course).Excluding(p => p.PrerequisiteCourse));
    }

    [Fact]
    public async Task GetListAsync_WhenNoPrerequisitesExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new PrerequisiteRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenPrerequisiteExists_ShouldReturnCorrectPrerequisite()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PrerequisiteRepository(context);
        var expectedPrerequisite = _testPrerequisites.First();

        // Act
        var result = await repo.GetByIdAsync(expectedPrerequisite.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedPrerequisite, 
            options => options.Excluding(p => p.Course).Excluding(p => p.PrerequisiteCourse));
        result!.Course.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByCourseIdAsync_WhenPrerequisiteExists_ShouldReturnCorrectPrerequisite()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PrerequisiteRepository(context);
        var expectedPrerequisite = _testPrerequisites.First();

        // Act
        var result = await repo.GetByCourseIdAsync(expectedPrerequisite.CourseId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedPrerequisite, 
            options => options.Excluding(p => p.Course).Excluding(p => p.PrerequisiteCourse));
        result!.Course.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByCourseIdAsync_WhenPrerequisiteDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PrerequisiteRepository(context);
        const int nonExistentCourseId = -1;

        // Act
        var result = await repo.GetByCourseIdAsync(nonExistentCourseId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenPrerequisiteIsValid_ShouldAddAndSavePrerequisite()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PrerequisiteRepository(context);
        
        var availableCourses = _testCourses
            .Where(c => _testPrerequisites.All(p => 
                p.CourseId != c.Id || p.PrerequisiteCourseId != c.Id))
            .Take(2)
            .ToList();

        var newPrerequisite = new Prerequisite
        {
            CourseId = availableCourses[0].Id,
            Course = availableCourses[0],
            PrerequisiteCourseId = availableCourses[1].Id,
            PrerequisiteCourse = availableCourses[1],
            MinimumGrade = 3.0m
        };

        // Act
        await repo.AddAsync(newPrerequisite);
        var result = await context.Prerequisites
            .FirstOrDefaultAsync(p => p.CourseId == newPrerequisite.CourseId && 
                                   p.PrerequisiteCourseId == newPrerequisite.PrerequisiteCourseId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newPrerequisite, 
            options => options.Excluding(p => new { p.Course, p.PrerequisiteCourse } ));
    }

    [Fact]
    public async Task DeleteForCourseAsync_WhenPrerequisiteExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PrerequisiteRepository(context);
        var prerequisiteToDelete = _testPrerequisites.First();

        // Act
        var result = await repo.DeleteForCourseAsync(prerequisiteToDelete.CourseId);
        var deletedPrerequisite = await repo.GetByCourseIdAsync(prerequisiteToDelete.CourseId);

        // Assert
        result.Should().BeTrue();
        deletedPrerequisite.Should().BeNull();
    }

    [Fact]
    public async Task DeleteForCourseAsync_WhenPrerequisiteDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PrerequisiteRepository(context);
        const int nonExistentCourseId = -1;

        // Act
        var result = await repo.DeleteForCourseAsync(nonExistentCourseId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DoesExistsAsync_WhenPrerequisiteExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PrerequisiteRepository(context);
        var existingPrerequisite = _testPrerequisites.First();

        // Act
        var result = await repo.DoesExistsAsync(
            existingPrerequisite.CourseId, 
            existingPrerequisite.PrerequisiteCourseId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DoesExistsAsync_WhenPrerequisiteDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PrerequisiteRepository(context);
        const int nonExistentCourseId = -1;
        const int nonExistentPrerequisiteCourseId = -2;

        // Act
        var result = await repo.DoesExistsAsync(
            nonExistentCourseId, 
            nonExistentPrerequisiteCourseId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DoesExistsAsync_WhenSameCourseAndPrerequisite_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new PrerequisiteRepository(context);
        var course = _testCourses.First();

        // Act
        var result = await repo.DoesExistsAsync(course.Id, course.Id);

        // Assert
        result.Should().BeFalse();
    }
}