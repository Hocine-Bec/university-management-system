using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Defines course prerequisites with minimum grade requirements for academic progression
/// </summary>
public class Prerequisite : IEntity
{
    public int Id { get; set; }
    public decimal MinimumGrade { get; set; }
    
    public int CourseId { get; set; }
    public required Course Course { get; set; } 
    
    public int PrerequisiteCourseId { get; set; }
    public required Course PrerequisiteCourse { get; set; }
}