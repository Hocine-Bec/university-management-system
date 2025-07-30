using Applications.DTOs.Semester;
using FluentValidation;
using System.Collections.Generic;

namespace Applications.Validators;

public class SemesterRequestValidator : AbstractValidator<SemesterRequest>
{
    private static readonly List<string> ValidTerms = new() { "Fall", "Spring", "Summer" };

    public SemesterRequestValidator()
    {
        RuleFor(x => x.Term)
            .NotEmpty().WithMessage("Term is required.")
            .Must(BeAValidTerm).WithMessage("Term must be one of the following: Fall, Spring, Summer.")
            .When(x => !string.IsNullOrEmpty(x.Term));

        RuleFor(x => x.Year)
            .NotNull().WithMessage("Year is required.")
            .InclusiveBetween(DateTime.Now.Year - 1, DateTime.Now.Year + 5).WithMessage("Year must be within a valid range.");

        RuleFor(x => x.StartDate)
            .NotNull().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotNull().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after the start date.")
            .When(x => x.StartDate.HasValue);

        RuleFor(x => x.RegStartsAt)
            .NotNull().WithMessage("Registration start date is required.")
            .LessThan(x => x.StartDate).WithMessage("Registration must start before the semester begins.")
            .When(x => x.StartDate.HasValue);

        RuleFor(x => x.RegEndsAt)
            .NotNull().WithMessage("Registration end date is required.")
            .GreaterThan(x => x.RegStartsAt).WithMessage("Registration end date must be after the registration start date.")
            .LessThan(x => x.EndDate).WithMessage("Registration must end before the semester ends.")
            .When(x => x.RegStartsAt.HasValue && x.EndDate.HasValue);

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive status is required.");
    }

    private bool BeAValidTerm(string? term)
    {
        return !string.IsNullOrEmpty(term) && ValidTerms.Contains(term);
    }
}
