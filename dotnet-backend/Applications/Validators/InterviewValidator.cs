using Applications.DTOs.Interview;
using FluentValidation;

namespace Applications.Validators;

public class InterviewRequestValidator : AbstractValidator<InterviewRequest>
{
    public InterviewRequestValidator()
    {
        RuleFor(x => x.ScheduledDate)
            .NotNull().WithMessage("Scheduled date is required.")
            .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("Scheduled date cannot be in the past.");

        RuleFor(x => x.StartTime)
            .NotNull().WithMessage("Start time is required.")
            .When(x => x.StartTime.HasValue && x.ScheduledDate.HasValue);

        RuleFor(x => x.EndTime)
            .NotNull().WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after the start time.")
            .When(x => x.EndTime.HasValue && x.StartTime.HasValue);

        RuleFor(x => x.PaidFees)
            .GreaterThanOrEqualTo(0).WithMessage("Paid fees cannot be negative.")
            .When(x => x.PaidFees.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x.Recommendation)
            .MaximumLength(2000).WithMessage("Recommendation cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Recommendation));

        RuleFor(x => x.ProfessorId)
            .NotNull().WithMessage("Professor ID is required.")
            .GreaterThan(0).WithMessage("Professor ID must be a valid positive number.");
    }
}

public class CompleteInterviewRequestValidator : AbstractValidator<CompleteInterviewRequest>
{
    public CompleteInterviewRequestValidator()
    {
        RuleFor(x => x.EndTime)
            .NotNull().WithMessage("End time is required.");

        RuleFor(x => x.IsApproved)
            .NotNull().WithMessage("Approval status is required.");

        RuleFor(x => x.Recommendation)
            .NotEmpty().WithMessage("Recommendation is required.")
            .MinimumLength(20).WithMessage("Recommendation must be at least 20 characters.")
            .MaximumLength(2000).WithMessage("Recommendation cannot exceed 2000 characters.");
    }
}
