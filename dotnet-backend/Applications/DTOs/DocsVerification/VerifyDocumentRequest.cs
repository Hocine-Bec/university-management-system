namespace Applications.DTOs.DocsVerification;

public record struct VerifyDocumentRequest
{
    public int? UserId { get; set; }
    public bool? IsApproved { get; set; }
    public string? Notes { get; set; }
}