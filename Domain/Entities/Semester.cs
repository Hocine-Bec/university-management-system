using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Academic semester/term with registration periods and course scheduling
/// </summary>
public class Semester : IEntity
{
    public int Id { get; set; }
    public required string TermCode { get; set; }
    public required string Term { get; set; }
    public required int Year { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? RegStartsAt { get; set; }
    public DateTime? RegEndsAt { get; set; }
    public bool IsActive { get; set; }
}