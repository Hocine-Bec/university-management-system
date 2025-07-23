using Domain.Enums;

namespace Applications.DTOs.EntranceExam;

public record struct EntranceExamRequest
{
    public DateTime? ExamDate { get; set; }
    public decimal? Score { get; set; }
    public bool? IsPassed { get; set; }
    public decimal? PaidFees { get; set; }
    public ExamStatus? ExamStatus { get; set; }
    public string? Notes { get; set; }
}