namespace Applications.DTOs.Course;

public record struct CourseRequest
{
    public string? Code { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? CreditHours { get; set; }
    public bool? IsActive { get; set; }
}