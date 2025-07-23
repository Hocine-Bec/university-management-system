using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Represents a country for nationality tracking
/// </summary>
public class Country : IEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
}