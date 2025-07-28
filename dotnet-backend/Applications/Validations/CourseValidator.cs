using Applications.DTOs.Course;
using FluentValidation;

namespace Applications.Validations;

public class CourseValidator : AbstractValidator<CourseRequest>
{
    public CourseValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Course code is required")
            .MaximumLength(10).WithMessage("Course code cannot exceed 10 characters");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Course title is required")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters long")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.CreditHours)
            .GreaterThan(0).WithMessage("Credit hours must be a positive number")
            .When(x => x.CreditHours.HasValue);

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive flag is required");
    }
}
