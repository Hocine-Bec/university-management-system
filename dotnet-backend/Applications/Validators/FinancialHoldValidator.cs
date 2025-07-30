using Applications.DTOs.FinancialHold;
using FluentValidation;

namespace Applications.Validators;

public class FinancialHoldRequestValidator : AbstractValidator<FinancialHoldRequest>
{
    public FinancialHoldRequestValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason for the hold is required.")
            .MaximumLength(200).WithMessage("Reason cannot exceed 200 characters.");

        RuleFor(x => x.HoldAmount)
            .NotNull().WithMessage("Hold amount is required.")
            .GreaterThan(0).WithMessage("Hold amount must be a positive value.");

        RuleFor(x => x.DatePlaced)
            .NotNull().WithMessage("Date placed is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Date placed cannot be in the future.");

        RuleFor(x => x.DateResolved)
            .GreaterThanOrEqualTo(x => x.DatePlaced).WithMessage("Resolution date must be on or after the date the hold was placed.")
            .When(x => x.DateResolved.HasValue);

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive status is required.");

        RuleFor(x => x.ResolutionNotes)
            .MaximumLength(500).WithMessage("Resolution notes cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.ResolutionNotes));

        RuleFor(x => x.StudentId)
            .NotNull().WithMessage("Student ID is required.")
            .GreaterThan(0).WithMessage("Student ID must be a valid positive number.");

        RuleFor(x => x.PlacedByUserId)
            .NotNull().WithMessage("User ID of the placer is required.")
            .GreaterThan(0).WithMessage("Placer's User ID must be a valid positive number.");

        RuleFor(x => x.ResolvedByUserId)
            .GreaterThan(0).WithMessage("Resolver's User ID must be a valid positive number.")
            .When(x => x.ResolvedByUserId.HasValue);
    }
}

public class ResolveRequestValidator : AbstractValidator<ResolveRequest>
{
    public ResolveRequestValidator()
    {
        RuleFor(x => x.ResolutionNotes)
            .NotEmpty().WithMessage("Resolution notes are required to resolve a hold.")
            .MinimumLength(10).WithMessage("Resolution notes must be at least 10 characters.")
            .MaximumLength(500).WithMessage("Resolution notes cannot exceed 500 characters.");

        RuleFor(x => x.ResolvedByUserId)
            .NotNull().WithMessage("Resolver's User ID is required.")
            .GreaterThan(0).WithMessage("Resolver's User ID must be a valid positive number.");
    }
}
