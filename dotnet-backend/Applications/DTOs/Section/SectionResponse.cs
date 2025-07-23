namespace Applications.DTOs.Section;

public record struct SectionResponse
{
    public int Id { get; set; }
    public required string SectionNumber { get; set; }
    public string MeetingDays { get; set; }
    public string TimeSlot { get; set; }
    public string Classroom { get; set; }
    public int MaxCapacity { get; set; }
    public int? CurrentEnrollment { get; set; }
    public int CourseId { get; set; }
    public int SemesterId { get; set; }
    public int ProfessorId { get; set; }
}