using Domain.Enums;

namespace Applications.DTOs.Student;

public record struct StudentRequest
{
    public string? StudentNumber { get; set; }
    public StudentStatus? StudentStatus { get; set; }
    public string? Notes { get; set; }
    public int PersonId { get; set; }
}