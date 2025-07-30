using Applications.DTOs.Grade;
using FluentValidation;

namespace Applications.Validators;

public class GradeRequestValidator : AbstractValidator<GradeRequest>
{
    public GradeRequestValidator()
    {
        RuleFor(x => x.Score)
            .NotNull().WithMessage("Score is required.")
            .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.");

        RuleFor(x => x.DateRecorded)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Date recorded cannot be in the future.")
            .When(x => x.DateRecorded.HasValue);

        RuleFor(x => x.Comments)
            .MaximumLength(500).WithMessage("Comments cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Comments));

        RuleFor(x => x.StudentId)
            .NotNull().WithMessage("Student ID is required.")
            .GreaterThan(0).WithMessage("Student ID must be a valid positive number.");

        RuleFor(x => x.CourseId)
            .NotNull().WithMessage("Course ID is required.")
            .GreaterThan(0).WithMessage("Course ID must be a valid positive number.");

        RuleFor(x => x.SemesterId)
            .NotNull().WithMessage("Semester ID is required.")
            .GreaterThan(0).WithMessage("Semester ID must be a valid positive number.");

        RuleFor(x => x.RegistrationId)
            .NotNull().WithMessage("Registration ID is required.")
            .GreaterThan(0).WithMessage("Registration ID must be a valid positive number.");
    }
}
