using Applications.DTOs.Section;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Applications.Validators;

public class SectionRequestValidator : AbstractValidator<SectionRequest>
{
    public SectionRequestValidator()
    {
        RuleFor(x => x.SectionNumber)
            .NotEmpty().WithMessage("Section number is required.")
            .MaximumLength(10).WithMessage("Section number cannot exceed 10 characters.");

        RuleFor(x => x.MeetingDays)
            .NotEmpty().WithMessage("Meeting days are required.")
            .Must(BeValidMeetingDays).WithMessage("Invalid meeting days format. Use a combination of M, T, W, R, F, S, U.");

        RuleFor(x => x.StartTime)
            .NotNull().WithMessage("Start time is required.");

        RuleFor(x => x.EndTime)
            .NotNull().WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.")
            .When(x => x.StartTime.HasValue);

        RuleFor(x => x.Classroom)
            .MaximumLength(50).WithMessage("Classroom cannot exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.Classroom));

        RuleFor(x => x.MaxCapacity)
            .NotNull().WithMessage("Maximum capacity is required.")
            .GreaterThan(0).WithMessage("Maximum capacity must be a positive number.");

        RuleFor(x => x.CurrentEnrollment)
            .GreaterThanOrEqualTo(0).WithMessage("Current enrollment cannot be negative.")
            .LessThanOrEqualTo(x => x.MaxCapacity).WithMessage("Current enrollment cannot exceed maximum capacity.")
            .When(x => x.CurrentEnrollment.HasValue && x.MaxCapacity.HasValue);

        RuleFor(x => x.CourseId)
            .NotNull().WithMessage("Course ID is required.")
            .GreaterThan(0).WithMessage("Course ID must be a valid positive number.");

        RuleFor(x => x.SemesterId)
            .NotNull().WithMessage("Semester ID is required.")
            .GreaterThan(0).WithMessage("Semester ID must be a valid positive number.");

        RuleFor(x => x.ProfessorId)
            .NotNull().WithMessage("Professor ID is required.")
            .GreaterThan(0).WithMessage("Professor ID must be a valid positive number.");
    }

    private bool BeValidMeetingDays(string? meetingDays)
    {
        if (string.IsNullOrEmpty(meetingDays)) return false;
        return Regex.IsMatch(meetingDays, "^[MTWRFSU]+$");
    }
}
