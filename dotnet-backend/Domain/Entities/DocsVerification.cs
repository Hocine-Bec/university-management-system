using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Document verification step in admission process with $25 fee and approval tracking
/// </summary>
public class DocsVerification : IEntity
{
    public int Id { get; set; }
    public DateTime SubmissionDate { get; set; }
    public DateTime? VerificationDate { get; set; }
    public VerificationStatus Status { get; set; }
    public bool? IsApproved { get; set; }
    public string? RejectedReason { get; set; }
    public decimal PaidFees { get; set; }
    public string? Notes { get; set; }
    
    public int PersonId { get; set; }
    public required Person Person { get; set; }
    
    public int? VerifiedByUserId { get; set; }
    public User? VerifiedByUser { get; set; }
}