using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Central application entity for requesting university services with fee tracking and approval workflow
/// </summary>
public class ServiceApplication : IEntity
{
    public int Id { get; set; }
    public DateTime ApplicationDate { get; set; }
    public ApplicationStatus Status { get; set; }
    public decimal PaidFees { get; set; }
    public string? Notes { get; set; }
    public DateTime? CompletedDate { get; set; }
    
    public int PersonId { get; set; }
    public required Person Person { get; set; } 
    
    public int ServiceOfferId { get; set; }
    public required ServiceOffer ServiceOffer { get; set; } 
    
    public int? ProcessedByUserId { get; set; }
    public User? ProcessedByUser { get; set; }
}