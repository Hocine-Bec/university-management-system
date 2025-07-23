using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Financial holds preventing student registration and services until outstanding balances are resolved
/// </summary>
public class FinancialHold : IEntity
{
    public int Id { get; set; }
    public required string Reason { get; set; }
    public decimal HoldAmount { get; set; }
    public DateTime DatePlaced { get; set; }
    public DateTime? DateResolved { get; set; }
    public bool IsActive { get; set; }
    public string? ResolutionNotes { get; set; }
    
    public int StudentId { get; set; }
    public required Student Student { get; set; } 
    
    public int PlacedByUserId { get; set; }
    public required User PlacedByUser { get; set; } 
    
    public int? ResolvedByUserId { get; set; }
    public User? ResolvedByUser { get; set; }
}