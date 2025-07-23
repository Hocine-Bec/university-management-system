using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Specific course offering with schedule, capacity, and instructor assignment
/// </summary>
public class Section : IEntity
{
    public int Id { get; set; }
    public required string SectionNumber { get; set; }
    public string? MeetingDays { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public string? Classroom { get; set; }
    public int MaxCapacity { get; set; }
    public int? CurrentEnrollment { get; set; }
    
    public int CourseId { get; set; }
    public required Course Course { get; set; }
    
    public int SemesterId { get; set; }
    public required Semester Semester { get; set; } 
    
    public int? ProfessorId { get; set; }
    public Professor? Professor { get; set; }
}