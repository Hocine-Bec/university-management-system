using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Student registration for course sections with fee tracking and semester-specific enrollment
/// </summary>
public class Registration : IEntity
{
    public int Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public decimal RegistrationFees { get; set; }
    
    public int StudentId { get; set; }
    public required Student Student { get; set; } 
    
    public int SectionId { get; set; }
    public required Section Section { get; set; }
    
    public int SemesterId { get; set; }
    public required Semester Semester { get; set; }
    
    public int? ProcessedByUserId { get; set; }
    public User? ProcessedByUser { get; set; }
}