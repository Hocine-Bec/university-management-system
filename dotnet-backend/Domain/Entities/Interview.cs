using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Admission interview conducted by faculty with $40 fee and approval recommendation
/// </summary>
public class Interview : IEntity
{
    public int Id { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool IsApproved { get; set; }
    public decimal PaidFees { get; set; } 
    public string? Notes { get; set; }
    public string? Recommendation { get; set; }

    public int? ProfessorId { get; set; }
    public Professor? Professor { get; set; }
}