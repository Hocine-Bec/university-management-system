using Domain.Entities;
using Domain.Enums;

namespace Applications.DTOs.DocsVerification;

public record struct DocsVerificationRequest
{
    public DateTime? SubmissionDate { get; set; }
    public DateTime? VerificationDate { get; set; }
    public VerificationStatus? Status { get; set; }
    public bool? IsApproved { get; set; }
    public string? RejectedReason { get; set; }
    public decimal? PaidFees { get; set; }
    public string? Notes { get; set; }
    public int? PersonId { get; set; }
    public int? VerifiedByUserId { get; set; }
}

