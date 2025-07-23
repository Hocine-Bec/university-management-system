using Domain.Enums;

namespace Applications.DTOs.Student
{
    public record struct StudentResponse
    {
        public int Id { get; set; }
        public required string StudentNumber { get; set; }
        public string Status { get; set; }
        public string PersonFullName { get; set; }
    }
}
