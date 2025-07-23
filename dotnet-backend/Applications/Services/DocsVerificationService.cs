using Applications.DTOs.DocsVerification;
using Applications.Interfaces.Logging;
using Applications.Interfaces.Repositories;
using Applications.Interfaces.Services;
using Applications.Shared;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Applications.Services;

public class DocsVerificationService : IDocsVerificationService
{
    private readonly IDocsVerificationRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMyLogger _logger;

    public DocsVerificationService(IDocsVerificationRepository repository, IMapper mapper, IMyLogger logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<Result<IReadOnlyCollection<DocsVerificationResponse>>> GetListAsync()
    {
        try
        {
            var verifications = await _repository.GetListAsync();
            if (!verifications.Any())
            {
                return Result<IReadOnlyCollection<DocsVerificationResponse>>.Failure(
                    "No document verifications found", ErrorType.NotFound);
            }

            var response = _mapper.Map<IReadOnlyCollection<DocsVerificationResponse>>(verifications);
            return Result<IReadOnlyCollection<DocsVerificationResponse>>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving document verifications", ex);
            return Result<IReadOnlyCollection<DocsVerificationResponse>>.Failure(
                "Failed to retrieve document verifications", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<DocsVerificationResponse>> GetByIdAsync(int id)
    {
        if (id <= 0)
            return Result<DocsVerificationResponse>.Failure("Invalid verification ID", ErrorType.BadRequest);

        try
        {
            var verification = await _repository.GetByIdAsync(id);
            if (verification == null)
                return Result<DocsVerificationResponse>.Failure("Document verification not found", ErrorType.NotFound);

            var response = _mapper.Map<DocsVerificationResponse>(verification);
            return Result<DocsVerificationResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving document verification", ex, new { id });
            return Result<DocsVerificationResponse>.Failure(
                "Failed to retrieve document verification", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<DocsVerificationResponse>> GetByPersonIdAsync(int personId)
    {
        if (personId <= 0)
            return Result<DocsVerificationResponse>.Failure("Invalid Person ID", ErrorType.BadRequest);
        
        try
        {
            var verification = await _repository.GetByPersonIdAsync(personId);
            if (verification == null)
            {
                return Result<DocsVerificationResponse>.Failure(
                    $"No document verifications found for person with ID [{personId}]", ErrorType.NotFound);
            }

            var response = _mapper.Map<DocsVerificationResponse>(verification);
            return Result<DocsVerificationResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error retrieving person's document verifications", ex, new { personId });
            return Result<DocsVerificationResponse>.Failure(
                "Failed to retrieve document verifications", ErrorType.InternalServerError);
        }
    }
    public async Task<Result<DocsVerificationResponse>> CreateAsync(DocsVerificationRequest request)
    {
        if (request == default)
            return Result<DocsVerificationResponse>.Failure("Verification data is required", ErrorType.BadRequest);

        try
        {
            var verification = _mapper.Map<DocsVerification>(request);
            
            if(request.SubmissionDate != null)
                verification.SubmissionDate = DateTime.UtcNow;
            
            if(request.Status == null)
                verification.Status = VerificationStatus.Pending;

            int id = await _repository.AddAsync(verification);
            if (id <= 0)
                return Result<DocsVerificationResponse>.Failure("Failed to create document verification", ErrorType.BadRequest);

            var response = _mapper.Map<DocsVerificationResponse>(verification);
            return Result<DocsVerificationResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error creating document verification", ex, new { request });
            return Result<DocsVerificationResponse>.Failure(
                "Failed to create document verification", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> UpdateAsync(int id, DocsVerificationRequest request)
    {
        if (request == default)
            return Result.Failure("Verification data is required", ErrorType.BadRequest);

        try
        {
            var existingVerification = await _repository.GetByIdAsync(id);
            if (existingVerification == null)
                return Result.Failure("Document verification not found", ErrorType.NotFound);

            _mapper.Map(request, existingVerification);

            bool isUpdated = await _repository.UpdateAsync(existingVerification);
            return !isUpdated ? Result.Failure("Failed to update document verification", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error updating document verification", ex, new { id, request });
            return Result.Failure("Failed to update document verification", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> VerifyDocumentAsync(int id, VerifyDocumentRequest request)
    {
        if (id <= 0)
            return Result.Failure("Invalid ID Provided", ErrorType.BadRequest);

        if(request == default)
            return Result.Failure("Verification data is required", ErrorType.BadRequest);
            
        try
        {
            var verification = await _repository.GetByIdAsync(id);
            if(verification == null)
                return Result.Failure("No Document Verification Found", ErrorType.NotFound);
            
            verification.VerificationDate = DateTime.UtcNow;
            verification.VerifiedByUserId = request.UserId;
            verification.Status = request.IsApproved != null ? VerificationStatus.Approved : VerificationStatus.Rejected;
            verification.Notes = request.Notes;

            bool isUpdated = await _repository.UpdateAsync(verification);
            return !isUpdated ? Result.Failure("Failed to verify documents", ErrorType.BadRequest) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error verifying documents", ex, new { request });
            return Result.Failure("Failed to verify documents", ErrorType.InternalServerError);
        }
    }
    public async Task<Result> DeleteAsync(int id)
    {
        if (id <= 0)
            return Result.Failure("Invalid verification ID", ErrorType.BadRequest);

        try
        {
            bool isDeleted = await _repository.DeleteAsync(id);
            return !isDeleted ? Result.Failure("Document verification not found", ErrorType.NotFound) : Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError("Database error deleting document verification", ex, new { id });
            return Result.Failure("Failed to delete document verification", ErrorType.InternalServerError);
        }
    }
}