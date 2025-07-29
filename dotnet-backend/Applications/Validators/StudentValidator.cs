using Applications.DTOs.Student;
using FluentValidation;

namespace Applications.Validators;

public class StudentRequestValidator : AbstractValidator<StudentRequest>
{
    public StudentRequestValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("Person ID is required")
            .GreaterThan(0).WithMessage("Person ID must be a valid positive number");

        RuleFor(x => x.StudentNumber)
            .Matches(@"^[A-F0-9]{8}$")
            .WithMessage("Student number must be exactly 8 characters containing only uppercase letters (A-F) and numbers (0-9)")
            .When(x => !string.IsNullOrEmpty(x.StudentNumber));

        RuleFor(x => x.StudentStatus)
            .IsInEnum().WithMessage("Invalid student status")
            .When(x => x.StudentStatus.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

public class UpdateStudentStatusRequestValidator : AbstractValidator<UpdateStudentStatusRequest>
{
    public UpdateStudentStatusRequestValidator()
    {
        RuleFor(x => x.StudentStatus)
            .NotNull().WithMessage("Student status is required")
            .IsInEnum().WithMessage("Invalid student status value");

        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("Notes are required when updating student status")
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters")
            .MinimumLength(10).WithMessage("Notes must be at least 10 characters when provided");
    }
}