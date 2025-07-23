using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Faculty member entity extending Person with employment and academic ranking details
/// </summary>
public class Professor : IEntity
{
    public int Id { get; set; }
    public required string EmployeeNumber { get; set; }
    public AcademicRank AcademicRank { get; set; }
    public DateTime HireDate { get; set; }
    public string? Specialization { get; set; }
    public string? OfficeLocation { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; } 
    
    public int PersonId { get; set; }
    public required Person Person { get; set; }
}