namespace Applications.DTOs.Course;

public record struct CourseResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public int CreditHours { get; set; }
    public bool IsActive { get; set; }
}