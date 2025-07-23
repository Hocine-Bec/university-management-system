using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Actual student-program enrollment record (created after successful admission)
/// </summary>
public class Enrollment : IEntity
{
    public int Id { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime? ActualGradDate { get; set; }
    public EnrollmentStatus Status { get; set; } 
    public string? Notes { get; set; }
    
    public int StudentId { get; set; }  
    public required Student Student { get; set; }
    
    public int ProgramId { get; set; }
    public required Program Program { get; set; }
    
    public int ServiceApplicationId { get; set; }
    public required ServiceApplication ServiceApplication { get; set; }
}