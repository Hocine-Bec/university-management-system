using Applications.DTOs.DocsVerification;
using FluentValidation;
using Domain.Enums;

namespace Applications.Validators
{
    public class DocsVerificationRequestValidator : AbstractValidator<DocsVerificationRequest>
    {
        public DocsVerificationRequestValidator()
        {
            RuleFor(x => x.PersonId)
                .NotNull().WithMessage("Person ID is required")
                .GreaterThan(0).WithMessage("Person ID must be a valid positive number")
                .When(x => x.PersonId.HasValue);

            RuleFor(x => x.SubmissionDate)
                .NotNull().WithMessage("Submission date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Submission date cannot be in the future")
                .GreaterThan(DateTime.Now.AddYears(-10)).WithMessage("Submission date cannot be more than 10 years ago")
                .When(x => x.SubmissionDate.HasValue);

            RuleFor(x => x.VerificationDate)
                .GreaterThanOrEqualTo(x => x.SubmissionDate)
                .WithMessage("Verification date must be on or after submission date")
                .LessThanOrEqualTo(DateTime.Now.AddDays(1))
                .WithMessage("Verification date cannot be more than 1 day in the future")
                .When(x => x.VerificationDate.HasValue && x.SubmissionDate.HasValue);

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid verification status")
                .When(x => x.Status.HasValue);

            RuleFor(x => x.IsApproved)
                .Equal(true)
                .WithMessage("Document must be approved when status is Approved")
                .When(x => x.Status == VerificationStatus.Approved && x.IsApproved.HasValue);

            RuleFor(x => x.IsApproved)
                .Equal(false)
                .WithMessage("Document must not be approved when status is Rejected")
                .When(x => x.Status == VerificationStatus.Rejected && x.IsApproved.HasValue);

            RuleFor(x => x.RejectedReason)
                .NotEmpty().WithMessage("Rejection reason is required when document is rejected")
                .MinimumLength(10).WithMessage("Rejection reason must be at least 10 characters")
                .MaximumLength(500).WithMessage("Rejection reason cannot exceed 500 characters")
                .When(x => x.IsApproved.HasValue && x.IsApproved.Value == false);

            RuleFor(x => x.RejectedReason)
                .Empty().WithMessage("Rejection reason should not be provided when document is approved")
                .When(x => x.IsApproved.HasValue && x.IsApproved.Value == true);

            RuleFor(x => x.PaidFees)
                .GreaterThanOrEqualTo(0).WithMessage("Paid fees cannot be negative")
                .LessThan(10000).WithMessage("Paid fees seems unusually high, please verify")
                .When(x => x.PaidFees.HasValue);

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));

            RuleFor(x => x.VerifiedByUserId)
                .NotNull().WithMessage("Verifier user ID is required when document is verified")
                .GreaterThan(0).WithMessage("Verifier user ID must be valid")
                .When(x => (x.Status == VerificationStatus.Approved || x.Status == VerificationStatus.Rejected) 
                          && x.VerifiedByUserId.HasValue);
        }
    }

    public class VerifyDocumentRequestValidator : AbstractValidator<VerifyDocumentRequest>
    {
        public VerifyDocumentRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull().WithMessage("User ID is required for document verification")
                .GreaterThan(0).WithMessage("User ID must be a valid positive number");

            RuleFor(x => x.IsApproved)
                .NotNull().WithMessage("Approval decision is required");

            RuleFor(x => x.Notes)
                .NotEmpty().WithMessage("Notes are required when rejecting a document")
                .MinimumLength(10).WithMessage("Rejection notes must be at least 10 characters")
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
                .When(x => x.IsApproved.HasValue && x.IsApproved.Value == false);

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
                .When(x => x.IsApproved.HasValue && x.IsApproved.Value == true && !string.IsNullOrEmpty(x.Notes));

            RuleFor(x => x.Notes)
                .Must(ContainMeaningfulRejectionReason)
                .WithMessage("Rejection notes must contain specific reason for rejection")
                .When(x => x.IsApproved.HasValue && x.IsApproved.Value == false && !string.IsNullOrEmpty(x.Notes));
        }

        private bool ContainMeaningfulRejectionReason(string notes)
        {
            if (string.IsNullOrWhiteSpace(notes)) return false;

            var rejectionKeywords = new[] 
            { 
                "incomplete", "missing", "invalid", "expired", "unclear", 
                "document", "signature", "information", "quality", "requirement",
                "format", "legible", "authentic", "verified", "policy"
            };

            var lowerNotes = notes.ToLower();
            var meaningfulWordCount = rejectionKeywords.Count(keyword => lowerNotes.Contains(keyword));
            return meaningfulWordCount >= 1 && notes.Split(' ').Length >= 5;
        }
    }
}