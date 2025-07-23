using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Student academic performance tracking with course-specific grades.
/// </summary>
public class Grade : IEntity
{
    public int Id { get; set; }
    public decimal Score { get; set; }
    public DateTime? DateRecorded { get; set; }
    public string? Comments { get; set; }
    
    public int StudentId { get; set; }
    public required Student Student { get; set; }
    
    public int CourseId { get; set; }
    public required Course Course { get; set; }
    
    public int SemesterId { get; set; }
    public required Semester Semester { get; set; } 
    
    public int? RegistrationId { get; set; }
    public Registration? Registration { get; set; }
}