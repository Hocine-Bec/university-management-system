using Applications.DTOs.DocsVerification;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IDocsVerificationService
{
    Task<Result<IReadOnlyCollection<DocsVerificationResponse>>> GetListAsync();
    Task<Result<DocsVerificationResponse>> GetByIdAsync(int id);
    Task<Result<DocsVerificationResponse>> GetByPersonIdAsync(int personId);
    Task<Result<DocsVerificationResponse>> CreateAsync(DocsVerificationRequest request);
    Task<Result> UpdateAsync(int id, DocsVerificationRequest request);
    Task<Result> VerifyDocumentAsync(int id, VerifyDocumentRequest request);
    Task<Result> DeleteAsync(int id);
}