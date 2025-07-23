namespace Applications.DTOs.Enrollment;

public record struct EnrollmentResponse
{
    public int Id { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime? ActualGradDate { get; set; }
    public string StatusName { get; set; }
    public int StudentId { get; set; }  
    public string ProgramName { get; set; }
}