namespace Applications.DTOs.FinancialHold;

public record struct ResolveRequest
{
    public string ResolutionNotes { get; set; }
    public int ResolvedByUserId { get; set; }
}