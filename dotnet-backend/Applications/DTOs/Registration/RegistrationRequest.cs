namespace Applications.DTOs.Registration;

public record struct RegistrationRequest
{
    public DateTime? RegistrationDate { get; set; }
    public decimal? RegistrationFees { get; set; }
    public int? StudentId { get; set; }
    public int? SectionId { get; set; }
    public int? SemesterId { get; set; }
    public int? ProcessedByUserId { get; set; }
}