using Applications.DTOs.People;
using FluentValidation;

namespace Applications.Validations;

public class PersonRequestValidator : AbstractValidator<PersonRequest>
{
    public PersonRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MinimumLength(3).WithMessage("First name must be at least 3 characters long")
            .MaximumLength(30).WithMessage("First name cannot exceed 30 characters")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("First name can only contain letters and spaces");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MinimumLength(3).WithMessage("Last name must be at least 3 characters long")
            .MaximumLength(30).WithMessage("Last name cannot exceed 30 characters")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Last name can only contain letters and spaces");

        RuleFor(x => x.DOB)
            .NotEmpty().WithMessage("Date of birth is required")
            .Must(BeAValidAge).WithMessage("Person must be between 1 and 120 years old");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.CountryId)
            .NotNull().WithMessage("Country is required")
            .GreaterThan(0).WithMessage("Country must be selected");

        RuleFor(x => x.Address)
            .MaximumLength(200).WithMessage("Address cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.Address));
    }
    
    private bool BeAValidAge(DateTime? dateOfBirth)
    {
        if (!dateOfBirth.HasValue) return false;
            
        var age = DateTime.Today.Year - dateOfBirth.Value.Year;
        if (dateOfBirth.Value.Date > DateTime.Today.AddYears(-age)) 
            age--;
            
        return age is >= 1 and <= 120;
    }
}