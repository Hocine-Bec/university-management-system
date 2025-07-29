using Applications.DTOs.Prerequisite;
using FluentValidation;

namespace Applications.Validators;

public class PrerequisiteRequestValidator : AbstractValidator<PrerequisiteRequest>
{
    public PrerequisiteRequestValidator()
    {
        RuleFor(x => x.CourseId)
            .NotNull().WithMessage("Course ID is required.")
            .GreaterThan(0).WithMessage("Course ID must be a valid positive number.");

        RuleFor(x => x.PrerequisiteCourseId)
            .NotNull().WithMessage("Prerequisite Course ID is required.")
            .GreaterThan(0).WithMessage("Prerequisite Course ID must be a valid positive number.")
            .NotEqual(x => x.CourseId)
            .WithMessage("A course cannot be its own prerequisite.");

        RuleFor(x => x.MinimumGrade)
            .InclusiveBetween(0, 100).WithMessage("Minimum grade must be between 0 and 100.")
            .When(x => x.MinimumGrade.HasValue);
    }
}
