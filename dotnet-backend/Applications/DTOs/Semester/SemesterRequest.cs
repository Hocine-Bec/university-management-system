namespace Applications.DTOs.Semester;

public record struct SemesterRequest
{
    public int? Id { get; set; }
    public string? Term { get; set; }
    public int? Year { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? RegStartsAt { get; set; }
    public DateTime? RegEndsAt { get; set; }
    public bool? IsActive { get; set; }
}