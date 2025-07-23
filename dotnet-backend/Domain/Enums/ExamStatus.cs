namespace Domain.Enums;

/// <summary>
/// Status of entrance exam
/// </summary>
public enum ExamStatus
{
    Scheduled = 1,      // Exam is scheduled, student hasn't taken it yet
    InProgress,     // Exam is currently being taken (for online exams)
    Completed,      // Exam finished, awaiting scoring
    Scored,         // Score has been entered
    NoShow,         // Student didn't show up
    Cancelled       // Exam was cancelled (by student or admin)
}