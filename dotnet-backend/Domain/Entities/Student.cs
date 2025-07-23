using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Student entity extending Person with academic-specific properties. Manages enrollment status and academic progression.
/// </summary>
public class Student : IEntity
{
    public int Id { get; set; }
    public required string StudentNumber { get; set; }
    public StudentStatus StudentStatus { get; set; } 
    public string? Notes { get; set; }

    public int PersonId { get; set; }
    public required Person Person { get; set; }
}