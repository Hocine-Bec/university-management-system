using Applications.DTOs.Program;
using FluentValidation;

namespace Applications.Validators;

public class ProgramValidator : AbstractValidator<ProgramRequest>
{
    public ProgramValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Program code is required")
            .MaximumLength(10).WithMessage("Program code cannot exceed 10 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Program name is required")
            .MinimumLength(5).WithMessage("Program name must be at least 5 characters long")
            .MaximumLength(100).WithMessage("Program name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.MinimumAge)
            .GreaterThan(0).WithMessage("Minimum age must be a positive number")
            .When(x => x.MinimumAge.HasValue);

        RuleFor(x => x.Duration)
            .GreaterThan(0).WithMessage("Duration must be a positive number")
            .When(x => x.Duration.HasValue);

        RuleFor(x => x.Fees)
            .GreaterThanOrEqualTo(0).WithMessage("Fees cannot be negative")
            .When(x => x.Fees.HasValue);

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive flag is required");
    }
}
