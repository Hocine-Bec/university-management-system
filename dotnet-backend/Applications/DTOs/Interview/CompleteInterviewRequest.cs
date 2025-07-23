namespace Applications.DTOs.Interview;

public record struct CompleteInterviewRequest
{
    public DateTime EndTime { get; set; }
    public bool IsApproved { get; set; }
    public string Recommendation { get; set; }
}