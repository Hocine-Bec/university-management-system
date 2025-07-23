using Domain.Enums;

namespace Applications.DTOs.Enrollment;

public record struct EnrollmentRequest
{
    public DateTime? EnrollmentDate { get; set; }
    public DateTime? ActualGradDate { get; set; }
    public EnrollmentStatus? Status { get; set; }
    public string? Notes { get; set; }
    public int? StudentId { get; set; }  
    public int? ProgramId { get; set; }
    public int? ServiceApplicationId { get; set; }
}