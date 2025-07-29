using Applications.DTOs.Registration;
using FluentValidation;

namespace Applications.Validators;

public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
{
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.RegistrationDate)
            .NotNull().WithMessage("Registration date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Registration date cannot be in the future.");

        RuleFor(x => x.RegistrationFees)
            .NotNull().WithMessage("Registration fees are required.")
            .GreaterThanOrEqualTo(0).WithMessage("Registration fees cannot be negative.");

        RuleFor(x => x.StudentId)
            .NotNull().WithMessage("Student ID is required.")
            .GreaterThan(0).WithMessage("Student ID must be a valid positive number.");

        RuleFor(x => x.SectionId)
            .NotNull().WithMessage("Section ID is required.")
            .GreaterThan(0).WithMessage("Section ID must be a valid positive number.");

        RuleFor(x => x.SemesterId)
            .NotNull().WithMessage("Semester ID is required.")
            .GreaterThan(0).WithMessage("Semester ID must be a valid positive number.");

        RuleFor(x => x.ProcessedByUserId)
            .NotNull().WithMessage("User ID of the processor is required.")
            .GreaterThan(0).WithMessage("Processor's User ID must be a valid positive number.");
    }
}
