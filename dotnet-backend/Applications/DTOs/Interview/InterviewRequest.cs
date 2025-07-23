namespace Applications.DTOs.Interview;

public record struct InterviewRequest
{
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public bool? IsApproved { get; set; }
    public decimal? PaidFees { get; set; } 
    public string? Notes { get; set; }
    public string? Recommendation { get; set; }
    public int? ProfessorId { get; set; }
}