namespace Applications.DTOs.FinancialHold;

public record struct FinancialHoldRequest
{
    public string? Reason { get; set; }
    public decimal? HoldAmount { get; set; }
    public DateTime? DatePlaced { get; set; }
    public DateTime? DateResolved { get; set; }
    public bool? IsActive { get; set; }
    public string? ResolutionNotes { get; set; }
    public int? StudentId { get; set; }
    public int? PlacedByUserId { get; set; }
    public int? ResolvedByUserId { get; set; }
}