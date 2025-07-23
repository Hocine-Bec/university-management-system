namespace Applications.DTOs.Prerequisite;

public record struct PrerequisiteResponse
{
    public int Id { get; set; }
    public decimal MinimumGrade { get; set; }
    public string CourseCode { get; set; }
    public string CourseTitle { get; set; }
    public string PrerequisiteCourseCode { get; set; }
    public string PrerequisiteCourseTitle { get; set; }
}