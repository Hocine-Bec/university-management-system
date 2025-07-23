namespace Applications.DTOs.Interview;

public record struct InterviewResponse
{
     public int Id { get; set; } 
     public DateTime? ScheduledDate { get; set; }
     public DateTime? StartTime { get; set; }
     public DateTime? EndTime { get; set; }
     public bool? IsApproved { get; set; }
     public decimal PaidFees { get; set; } 
     public string? ProfessorFullName { get; set; }
}