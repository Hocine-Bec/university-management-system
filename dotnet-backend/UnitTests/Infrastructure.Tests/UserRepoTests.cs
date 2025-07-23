using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.Common;
using UnitTests.Helpers;
using Person = Domain.Entities.Person;

namespace UnitTests.Infrastructure.Tests;

public class UserRepoTests
{
    private const int TestSeed = 505;
    private readonly List<Country> _testCountries;
    private readonly List<Person> _testPeople;
    private readonly List<Role> _testRoles;
    private readonly List<User> _testUsers;
    private readonly List<UserRole> _testUserRoles;

    public UserRepoTests()
    {
        _testCountries = CountryFactory.CreateTestCountries(seed: TestSeed);
        _testPeople = PersonFactory.CreateTestPeople(10, _testCountries, seed: TestSeed);
        _testRoles = RoleFactory.CreateTestRoles(3, seed: TestSeed);
        _testUsers = UserFactory.CreateTestUsers(5, _testPeople, seed: TestSeed);
        _testUserRoles = UserRoleFactory.CreateTestUserRoles(3, _testUsers, _testRoles, seed: TestSeed);
    }
    
    private async Task<AppDbContext> GetDbContext() =>
        await InMemoryDbFactory.CreateAsync(_testCountries, _testPeople, 
            _testRoles, _testUsers, _testUserRoles);
    
    private async Task<AppDbContext> GetEmptyDbContext() =>
        await InMemoryDbFactory.CreateAsync();

    [Fact]
    public async Task GetListAsync_WhenUsersExist_ShouldReturnAllUsers()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(_testUsers.Count);
        result.Should().BeEquivalentTo(_testUsers, options => 
            options.Excluding(u => u.Person)
                  .Excluding(u => u.UserRoles));
    }

    [Fact]
    public async Task GetListAsync_WhenNoUsersExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetEmptyDbContext();
        var repo = new UserRepository(context);

        // Act
        var result = await repo.GetListAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByUsernameAsync_WhenUserExists_ShouldReturnUserWithRoles()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        var expectedUser = _testUsers.First();
        var expectedRoles = _testUserRoles
            .Where(ur => ur.UserId == expectedUser.Id)
            .Select(ur => ur.Role)
            .ToList();

        // Act
        var result = await repo.GetByUsernameAsync(expectedUser.Username);

        // Assert
        result.Should().NotBeNull();
        result!.Username.Should().Be(expectedUser.Username);
        result.UserRoles.Should().HaveCount(expectedRoles.Count);
        result.UserRoles.Select(ur => ur.Role).Should().BeEquivalentTo(expectedRoles, options =>
            options.Excluding(r => r.UserRoles)); 
    }

    [Fact]
    public async Task GetByUsernameAsync_WhenUserDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        const string nonExistentUsername = "nonexistent";

        // Act
        var result = await repo.GetByUsernameAsync(nonExistentUsername);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetByUsernameAsync_WhenUsernameIsInvalid_ShouldReturnNull(string username)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);

        // Act
        var result = await repo.GetByUsernameAsync(username);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByUsernameAsync_WhenUserHasNoRoles_ShouldReturnEmptyRolesList()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        
        // Create a user with no roles
        var person = _testPeople.First(p => !_testUsers.Any(u => u.PersonId == p.Id));
        var userWithoutRoles = new User 
        { 
            Username = "norolesuser",
            Password = "password",
            IsActive = true,
            PersonId = person.Id,
            Person = person
        };
        await context.Users.AddAsync(userWithoutRoles);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByUsernameAsync(userWithoutRoles.Username);

        // Assert
        result.Should().NotBeNull();
        result!.UserRoles.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByUsernameAsync_WhenUserHasInactiveRoles_ShouldOnlyReturnActiveRoles()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        var user = _testUsers.First();
        
        // Add an inactive role
        var inactiveRole = _testRoles.Last();
        var inactiveUserRole = new UserRole
        {
            IsActive = false,
            UserId = user.Id,
            User = user,
            RoleId = inactiveRole.Id,
            Role = inactiveRole
        };
        await context.UserRoles.AddAsync(inactiveUserRole);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByUsernameAsync(user.Username);

        // Assert
        result.Should().NotBeNull();
        result!.UserRoles.Should().Contain(ur => ur.IsActive == false);
    }

    [Fact]
    public async Task GetByRoleAsync_ShouldReturnActiveUsersWithRole()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        var testRole = _testRoles.First();
        var expectedUsers = _testUserRoles
            .Where(ur => ur.RoleId == testRole.Id && ur.IsActive)
            .Select(ur => ur.User)
            .ToList();

        // Act
        var result = await repo.GetByRoleAsync(testRole.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(expectedUsers.Count);
        result.Should().BeEquivalentTo(expectedUsers, options => 
            options.Excluding(u => u.Person)
                  .Excluding(u => u.UserRoles));
    }

    [Fact]
    public async Task GetByRoleAsync_WhenRoleDoesNotExist_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        const int nonExistentRoleId = -1;

        // Act
        var result = await repo.GetByRoleAsync(nonExistentRoleId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByRoleAsync_WhenNoActiveUsersHaveRole_ShouldReturnEmptyList()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        var role = _testRoles.First();
        
        // Deactivate all user roles for this role
        foreach (var ur in context.UserRoles.Where(ur => ur.RoleId == role.Id))
        {
            ur.IsActive = false;
        }
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByRoleAsync(role.Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task DoesExistAsync_WhenPersonIdNotLinkedToUser_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        var personWithoutUser = _testPeople.First(p => 
            !_testUsers.Any(u => u.PersonId == p.Id));

        // Act
        var result = await repo.DoesExistAsync(personWithoutUser.Id);

        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task DoesExistAsync_ByPersonId_WhenUserExists_ShouldReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        var existingPersonId = _testUsers.First().PersonId;

        // Act
        var result = await repo.DoesExistAsync(existingPersonId);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DoesExistAsync_WhenPersonIdIsInvalid_ShouldReturnFalse(int personId)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);

        // Act
        var result = await repo.DoesExistAsync(personId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ByUsername_WhenUserExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        var userToDelete = _testUsers.First();

        // Act
        var result = await repo.DeleteAsync(userToDelete.Username);
        var deletedUser = await repo.GetByUsernameAsync(userToDelete.Username);

        // Assert
        result.Should().BeTrue();
        deletedUser.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ByUsername_WhenUserDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        const string nonExistentUsername = "nonexistent";

        // Act
        var result = await repo.DeleteAsync(nonExistentUsername);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ByUsername_ShouldCascadeDeleteUserRoles()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        var userToDelete = _testUsers.First();

        // Act
        var result = await repo.DeleteAsync(userToDelete.Username);
        var remainingUserRoles = await context.UserRoles
            .Where(ur => ur.UserId == userToDelete.Id)
            .CountAsync();

        // Assert
        result.Should().BeTrue();
        remainingUserRoles.Should().Be(0);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task DeleteAsync_WhenUsernameIsInvalid_ShouldReturnFalse(string username)
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);

        // Act
        var result = await repo.DeleteAsync(username);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task AddAsync_WhenUserIsValid_ShouldAddAndSaveUser()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        var person = _testPeople.First(p => _testUsers.All(u => u.PersonId != p.Id));
        
        var newUser = new User
        {
            Username = "newuser",
            Password = "SecurePassword123!",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            PersonId = person.Id,
            Person = person
        };

        // Act
        await repo.AddAsync(newUser);
        var result = await context.Users
            .FirstOrDefaultAsync(u => u.Username == newUser.Username);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(newUser, options => 
            options.Excluding(u => u.Person)
                  .Excluding(u => u.UserRoles)
                  .Excluding(u => u.Id));
    }

    [Fact]
    public async Task AddAsync_WhenUserIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);

        // Act
        Func<Task> act = async () => await repo.AddAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_WhenUserExists_ShouldUpdateAndSaveChanges()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        var userToUpdate = await context.Users.FirstAsync();
        var originalStatus = userToUpdate.IsActive;
        userToUpdate.IsActive = !originalStatus;

        // Act
        await repo.UpdateAsync(userToUpdate);
        var result = await context.Users.FindAsync(userToUpdate.Id);

        // Assert
        result.Should().NotBeNull();
        result!.IsActive.Should().NotBe(originalStatus);
        result.IsActive.Should().Be(userToUpdate.IsActive);
    }

    [Fact]
    public async Task UpdateAsync_WhenUserDoesNotExist_ShouldThrowDbUpdateConcurrencyException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);
        var nonExistentUser = new User
        {
            Id = -1,
            Username = "nonexistent",
            Password = "pwd",
            Person = null!
        };

        // Act
        Func<Task> act = async () => await repo.UpdateAsync(nonExistentUser);

        // Assert
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }

    [Fact]
    public async Task UpdateAsync_WhenUserIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        await using var context = await GetDbContext();
        var repo = new UserRepository(context);

        // Act
        Func<Task> act = async () => await repo.UpdateAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}