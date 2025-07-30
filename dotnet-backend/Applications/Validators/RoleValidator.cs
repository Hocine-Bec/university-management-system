using Applications.DTOs;
using FluentValidation;

namespace Applications.Validators;

public class RoleDtoValidator : AbstractValidator<RoleDto>
{
    public RoleDtoValidator()
    {
        RuleFor(x => x.Name)
            .IsInEnum().WithMessage("Invalid role name.");

        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}
