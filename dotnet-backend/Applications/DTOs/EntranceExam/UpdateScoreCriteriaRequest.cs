namespace Applications.DTOs.EntranceExam;

public record struct UpdateScoreCriteriaRequest
{
    public int MaxScore { get; set; }
    public int PassingScore { get; set; }
}