namespace Applications.DTOs.DocsVerification;

public record struct DocsVerificationResponse
{
    public int Id { get; set; }
    public DateTime SubmissionDate { get; set; }
    public DateTime? VerificationDate { get; set; }
    public string Status { get; set; }
    public bool IsApproved { get; set; }
    public string PersonFullName { get; set; }
}