namespace Applications.DTOs.Prerequisite;

public record struct PrerequisiteRequest
{
    public int? CourseId { get; set; }
    public int? PrerequisiteCourseId { get; set; }
    public decimal? MinimumGrade { get; set; }   
}