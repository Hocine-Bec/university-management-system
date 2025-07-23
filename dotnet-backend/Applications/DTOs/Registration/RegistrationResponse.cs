namespace Applications.DTOs.Registration;

public record struct RegistrationResponse
{
    public int Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public decimal RegistrationFees { get; set; }
    public string StudentNumber { get; set; }
    public string SectionNumber { get; set; }
    public string SemesterTermCode { get; set; }
    public int? ProcessedByUserId { get; set; }
}