namespace Applications.DTOs.EntranceExam;

public record struct UpdateScoreRequest
{
    public decimal? Score { get; set; }
    public string? Notes { get; set; }
}