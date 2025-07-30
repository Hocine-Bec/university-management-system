using Applications.DTOs.ServiceApplication;
using FluentValidation;

namespace Applications.Validators;

public class ServiceApplicationCreateRequestValidator : AbstractValidator<ServiceApplicationCreateRequest>
{
    public ServiceApplicationCreateRequestValidator()
    {
        RuleFor(x => x.PaidFees)
            .GreaterThanOrEqualTo(0).WithMessage("Paid fees cannot be negative.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x.CompletedDate)
            .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("Completed date cannot be in the past.")
            .When(x => x.CompletedDate.HasValue);

        RuleFor(x => x.PersonId)
            .GreaterThan(0).WithMessage("Person ID must be a valid positive number.");

        RuleFor(x => x.ServiceOfferId)
            .GreaterThan(0).WithMessage("Service Offer ID must be a valid positive number.");

        RuleFor(x => x.ProcessedByUserId)
            .GreaterThan(0).WithMessage("Processor User ID must be a valid positive number.");
    }
}

public class ServiceApplicationUpdateRequestValidator : AbstractValidator<ServiceApplicationUpdateRequest>
{
    public ServiceApplicationUpdateRequestValidator()
    {
        RuleFor(x => x.ApplicationDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Application date cannot be in the future.")
            .When(x => x.ApplicationDate.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid application status.")
            .When(x => x.Status.HasValue);

        RuleFor(x => x.PaidFees)
            .GreaterThanOrEqualTo(0).WithMessage("Paid fees cannot be negative.")
            .When(x => x.PaidFees.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x.CompletedDate)
            .GreaterThanOrEqualTo(x => x.ApplicationDate).WithMessage("Completed date must be on or after the application date.")
            .When(x => x.CompletedDate.HasValue && x.ApplicationDate.HasValue);

        RuleFor(x => x.PersonId)
            .GreaterThan(0).WithMessage("Person ID must be a valid positive number.")
            .When(x => x.PersonId.HasValue);

        RuleFor(x => x.ServiceOfferId)
            .GreaterThan(0).WithMessage("Service Offer ID must be a valid positive number.")
            .When(x => x.ServiceOfferId.HasValue);

        RuleFor(x => x.ProcessedByUserId)
            .GreaterThan(0).WithMessage("Processor User ID must be a valid positive number.")
            .When(x => x.ProcessedByUserId.HasValue);
    }
}

public class ServiceApplicationUpdateStatusRequestValidator : AbstractValidator<ServiceApplicationUpdateStatusRequest>
{
    public ServiceApplicationUpdateStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotNull().WithMessage("Application status is required.")
            .IsInEnum().WithMessage("Invalid application status.");
    }
}
