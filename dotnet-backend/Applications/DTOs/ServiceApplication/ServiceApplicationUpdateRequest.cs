using Domain.Enums;

namespace Applications.DTOs.ServiceApplication;

public record struct ServiceApplicationUpdateRequest
{
    public DateTime? ApplicationDate { get; set; }
    public ApplicationStatus? Status { get; set; }
    public decimal? PaidFees { get; set; }
    public string? Notes { get; set; }
    public DateTime? CompletedDate { get; set; }
    public int? PersonId { get; set; }
    public int? ServiceOfferId { get; set; }
    public int? ProcessedByUserId { get; set; }
}