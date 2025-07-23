namespace Applications.DTOs.EntranceExam;

public record struct EntranceExamResponse
{
    public int Id { get; set; }
    public DateTime ExamDate { get; set; }
    public decimal? Score { get; set; }
    public bool? IsPassed { get; set; }
    public decimal PaidFees { get; set; }
    public string ExamStatus { get; set; }
}