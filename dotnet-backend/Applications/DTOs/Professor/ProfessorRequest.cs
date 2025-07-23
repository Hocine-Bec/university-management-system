namespace Applications.DTOs.Professor;

public record struct ProfessorRequest
{
    public int? AcademicRank { get; set; }
    public DateTime? HireDate { get; set; }
    public string? Specialization { get; set; }
    public string? OfficeLocation { get; set; }
    public decimal? Salary { get; set; }
    public bool? IsActive { get; set; } 
    public int PersonId { get; set; }
}