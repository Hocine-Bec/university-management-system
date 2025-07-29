using Applications.DTOs.Enrollment;
using FluentValidation;

namespace Applications.Validators;

public class EnrollmentValidator : AbstractValidator<EnrollmentRequest>
{
    public EnrollmentValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student ID must be a valid positive number.")
            .When(x => x.StudentId.HasValue);

        RuleFor(x => x.ProgramId)
            .GreaterThan(0).WithMessage("Program ID must be a valid positive number.")
            .When(x => x.ProgramId.HasValue);
            
        RuleFor(x => x.ServiceApplicationId)
            .GreaterThan(0).WithMessage("Service Application ID must be a valid positive number.")
            .When(x => x.ServiceApplicationId.HasValue);

        RuleFor(x => x.EnrollmentDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Enrollment date cannot be in the future.")
            .When(x => x.EnrollmentDate.HasValue);

        RuleFor(x => x.ActualGradDate)
            .GreaterThan(x => x.EnrollmentDate).WithMessage("Actual graduation date must be after the enrollment date.")
            .When(x => x.ActualGradDate.HasValue && x.EnrollmentDate.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid enrollment status.")
            .When(x => x.Status.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
