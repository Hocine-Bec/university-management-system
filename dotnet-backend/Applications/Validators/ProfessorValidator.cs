using Applications.DTOs.Professor;
using FluentValidation;

namespace Applications.Validators;

public class ProfessorValidator : AbstractValidator<ProfessorRequest>
{
    public ProfessorValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("Person ID is required")
            .GreaterThan(0).WithMessage("Person ID must be a valid positive number");

        RuleFor(x => x.Salary)
            .GreaterThan(0).WithMessage("Salary must be a positive value")
            .When(x => x.Salary.HasValue);

        RuleFor(x => x.AcademicRank)
            .IsInEnum().WithMessage("Invalid academic rank")
            .When(x => x.AcademicRank.HasValue);

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Hire date cannot be in the future")
            .When(x => x.HireDate.HasValue);

        RuleFor(x => x.Specialization)
            .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Specialization));

        RuleFor(x => x.OfficeLocation)
            .MaximumLength(50).WithMessage("Office location cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.OfficeLocation));

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive flag is required");
    }
}
