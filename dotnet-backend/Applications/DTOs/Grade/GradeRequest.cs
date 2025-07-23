namespace Applications.DTOs.Grade;

public record struct GradeRequest
{
    public decimal? Score { get; set; }
    public DateTime? DateRecorded { get; set; }
    public string? Comments { get; set; }
    public int? StudentId { get; set; }
    public int? CourseId { get; set; }
    public int? SemesterId { get; set; }
    public int? RegistrationId { get; set; }
}