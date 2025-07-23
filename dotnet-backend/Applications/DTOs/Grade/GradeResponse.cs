namespace Applications.DTOs.Grade;

public record struct GradeResponse
{
    public int Id { get; set; }
    public decimal Score { get; set; }
    public DateTime? DateRecorded { get; set; }
    public string? Comments { get; set; }
    public string StudentNumber { get; set; }
    public string CourseTitle { get; set; }
    public string SemesterTermCode { get; set; }
    public int? RegistrationId { get; set; }
}