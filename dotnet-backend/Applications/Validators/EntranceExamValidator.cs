using Applications.DTOs.EntranceExam;
using FluentValidation;

namespace Applications.Validators;

public class EntranceExamRequestValidator : AbstractValidator<EntranceExamRequest>
{
    public EntranceExamRequestValidator()
    {
        RuleFor(x => x.ExamDate)
            .NotNull().WithMessage("Exam date is required.")
            .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("Exam date cannot be in the past.");

        RuleFor(x => x.Score)
            .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.")
            .When(x => x.Score.HasValue);

        RuleFor(x => x.PaidFees)
            .GreaterThanOrEqualTo(0).WithMessage("Paid fees cannot be negative.")
            .When(x => x.PaidFees.HasValue);

        RuleFor(x => x.ExamStatus)
            .IsInEnum().WithMessage("Invalid exam status.")
            .When(x => x.ExamStatus.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

public class UpdateScoreCriteriaRequestValidator : AbstractValidator<UpdateScoreCriteriaRequest>
{
    public UpdateScoreCriteriaRequestValidator()
    {
        RuleFor(x => x.MaxScore)
            .GreaterThan(0).WithMessage("Maximum score must be a positive number.");

        RuleFor(x => x.PassingScore)
            .GreaterThan(0).WithMessage("Passing score must be a positive number.")
            .LessThanOrEqualTo(x => x.MaxScore).WithMessage("Passing score cannot be greater than the maximum score.");
    }
}

public class UpdateScoreRequestValidator : AbstractValidator<UpdateScoreRequest>
{
    public UpdateScoreRequestValidator()
    {
        RuleFor(x => x.Score)
            .NotNull().WithMessage("Score is required.")
            .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
