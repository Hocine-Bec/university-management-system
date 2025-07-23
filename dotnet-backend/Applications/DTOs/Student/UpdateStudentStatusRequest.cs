using Domain.Enums;

namespace Applications.DTOs.Student
{
    public record struct UpdateStudentStatusRequest
    {
        public StudentStatus? StudentStatus { get; set; }
        public string Notes { get; set; }
    }
}
