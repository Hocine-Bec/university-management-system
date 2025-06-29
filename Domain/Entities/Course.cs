using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Course definition with credit hours and prerequisites management
/// </summary>
public class Course : IEntity
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int CreditHours { get; set; }
    public bool IsActive { get; set; } 
}