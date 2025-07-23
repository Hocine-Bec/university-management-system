using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;

namespace UnitTests.Infrastructure.Tests;

public class RoleRepositoryTests
{
    private const int TestSeed = 707;
    private readonly List<Role> _testRoles = RoleFactory.CreateTestRoles(Enum.GetValues(typeof(SystemRole)).Length, seed: TestSeed);

    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testRoles);
    
    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();

    [Fact]
    public async Task GetListAsync_WhenRolesExist_ShouldReturnAllRoles()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testRoles.Count);
        result.Should().BeEquivalentTo(_testRoles, options => 
            options.Excluding(r => r.UserRoles));
    }

    [Fact]
    public async Task GetListAsync_WhenNoRolesExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new RoleRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByIdAsync_WhenRoleExists_ShouldReturnCorrectRole()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);
        var expectedRole = _testRoles.First();

        // Act
        var result = await repo.GetByIdAsync(expectedRole.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedRole, options => 
            options.Excluding(r => r.UserRoles));
    }

    [Fact]
    public async Task GetByIdAsync_WhenRoleDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);
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
        var repo = new RoleRepository(context);

        // Act
        var result = await repo.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_WhenRoleIsValid_ShouldAddAndSaveRole()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);
        
        var newRole = new Role
        {
            Name = SystemRole.ItSupport,
            Description = "New IT Support Role"
        };
        
        // Act
        var result = await repo.AddAsync(newRole);
        var addedRole = await repo.GetByIdAsync(result);

        // Assert
        result.Should().BeGreaterThan(0); // Verify we got a valid ID
        addedRole.Should().NotBeNull();
        addedRole!.Id.Should().Be(result); // Verify returned ID matches the saved entity
        addedRole.Name.Should().Be(newRole.Name);
        addedRole.Description.Should().Be(newRole.Description);
    }

    [Fact]
    public async Task AddAsync_WhenRoleIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);

        // Act
        Func<Task> act = async () => await repo.AddAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_WhenRoleExists_ShouldUpdateAndSaveChanges()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);
        var roleToUpdate = await context.Roles.FirstAsync();
        var originalDescription = roleToUpdate.Description;
        roleToUpdate.Description = "Updated Description";

        // Act
        await repo.UpdateAsync(roleToUpdate);
        var result = await context.Roles.FindAsync(roleToUpdate.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Description.Should().NotBe(originalDescription);
        result.Description.Should().Be("Updated Description");
    }

    [Fact]
    public async Task UpdateAsync_WhenRoleDoesNotExist_ShouldThrowDbUpdateConcurrencyException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);
        var nonExistentRole = new Role { Id = -1, Name = SystemRole.Student };

        // Act
        Func<Task> act = async () => await repo.UpdateAsync(nonExistentRole);

        // Assert
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }

    [Fact]
    public async Task DeleteAsync_WhenRoleExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);
        var roleToDelete = _testRoles.First();

        // Act
        var result = await repo.DeleteAsync(roleToDelete.Id);
        var deletedRole = await repo.GetByIdAsync(roleToDelete.Id);

        // Assert
        result.Should().BeTrue();
        deletedRole.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WhenRoleDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);
        const int nonExistentId = -1;

        // Act
        var result = await repo.DeleteAsync(nonExistentId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetByNameAsync_WhenRoleExists_ShouldReturnCorrectRole()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);
        var expectedRole = _testRoles.First();

        // Act
        var result = await repo.GetByNameAsync(expectedRole.Name);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedRole, options => 
            options.Excluding(r => r.UserRoles));
    }

    [Fact]
    public async Task GetByNameAsync_WhenRoleDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);
        const SystemRole nonExistentRole = (SystemRole)999; // Value not in enum

        // Act
        var result = await repo.GetByNameAsync(nonExistentRole);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData((SystemRole)0)] // Invalid enum value
    [InlineData((SystemRole)(-1))] // Invalid enum value
    public async Task GetByNameAsync_WhenNameIsInvalid_ShouldReturnNull(SystemRole name)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);

        // Act
        var result = await repo.GetByNameAsync(name);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByNameAsync_WhenMultipleRolesSameName_ShouldReturnFirstMatch()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new RoleRepository(context);
        
        // Force duplicate name (shouldn't happen due to unique constraint but testing behavior)
        var duplicateRole = new Role 
        { 
            Name = _testRoles.First().Name,
            Description = "Duplicate Role" 
        };
        await context.Roles.AddAsync(duplicateRole);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByNameAsync(duplicateRole.Name);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(duplicateRole.Name);
    }
}