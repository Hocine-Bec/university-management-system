using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// University service definition with fees for the 8 core services offered
/// </summary>
public class ServiceOffer : IEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Fees { get; set; }
    public bool IsActive { get; set; }
}