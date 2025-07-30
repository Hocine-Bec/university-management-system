using Applications.DTOs.ServiceOffer;
using FluentValidation;

namespace Applications.Validators;

public class ServiceOfferRequestValidator : AbstractValidator<ServiceOfferRequest>
{
    public ServiceOfferRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Service offer name is required.")
            .MinimumLength(3).WithMessage("Service offer name must be at least 3 characters long.")
            .MaximumLength(100).WithMessage("Service offer name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Fees)
            .NotNull().WithMessage("Fees are required.")
            .GreaterThanOrEqualTo(0).WithMessage("Fees cannot be negative.");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive status is required.");
    }
}
