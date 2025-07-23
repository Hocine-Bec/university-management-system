using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Base entity for all individuals in the university system. Stores core personal information to prevent duplication across roles.
/// </summary>
public class Person : IEntity
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime DOB { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? ImagePath { get; set; }

    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;
}