using Applications.DTOs.UserRole;
using FluentValidation;

namespace Applications.Validators;

public class UserRoleDtoValidator : AbstractValidator<UserRoleDto>
{
    public UserRoleDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID must be a valid positive number.");

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("Role ID must be a valid positive number.");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive status is required.");
    }
}
