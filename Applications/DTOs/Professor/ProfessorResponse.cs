namespace Applications.DTOs.Professor;

public record struct ProfessorResponse
{
    public int Id { get; set; }
    public string EmployeeNumber { get; set; }
    public string AcademicRank { get; set; }
    public string? Specialization { get; set; }
    public string? OfficeLocation { get; set; }
    public bool IsActive { get; set; } 
    public string PersonFullName { get; set; }
}