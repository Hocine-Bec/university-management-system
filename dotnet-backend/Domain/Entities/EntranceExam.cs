using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Entrance examination with $50 fee, scoring out of 100, and pass/fail tracking
/// </summary>
public class EntranceExam : IEntity
{
    public int Id { get; set; }
    public DateTime ExamDate { get; set; }
    public decimal? Score { get; set; }
    public int MaxScore { get; set; }
    public int PassingScore { get; set; }
    public bool? IsPassed { get; set; }
    public decimal? PaidFees { get; set; }
    public ExamStatus ExamStatus { get; set; }
    public string? Notes { get; set; }
}
