using Applications.DTOs.Enrollment;
using Applications.DTOs.FinancialHold;
using Applications.DTOs.Prerequisite;
using Applications.Interfaces.UnitOfWorks;
using Applications.Shared;
using Domain.Enums;
using static Applications.Shared.Result;

namespace Applications.Helpers;

public static class RelationshipValidator
{
    public static async Task<Result> ValidateForEnrollmentAsync(this EnrollmentRequest request, IUnitOfWork uow)
    {
        var requirementsValidation = await ValidatePrerequisitesAsync(request, uow);
        
        if (!requirementsValidation.IsSuccess)
            return requirementsValidation;
            
        var canStudentEnroll = await uow.Enrollments.CanEnrollInProgramAsync(
            request.StudentId.Value, request.ProgramId.Value);
            
        if (!canStudentEnroll)
        {
            return Failure(
                "Student cannot enroll: already has an active, on-leave, or completed enrollment in this program", 
                ErrorType.Conflict);
        }
        
        return Success;
    }

    public static async Task<Result> ValidatePrerequisitesAsync(this EnrollmentRequest request, IUnitOfWork uow)
    {
        // Validate required fields
        if (!request.StudentId.HasValue || !request.ProgramId.HasValue || !request.ServiceApplicationId.HasValue)
            return Failure("Student ID, Program ID, and Service Application ID are required", ErrorType.BadRequest);
        
        // Validate entities exist
        var studentExists = await uow.Students.DoesExistsAsync(request.StudentId.Value);
        if (!studentExists)
            return Failure("Student not found", ErrorType.NotFound);

        var programExists = await uow.Programs.DoesExistsAsync(request.ProgramId.Value);
        if (!programExists)
            return Failure("Program not found", ErrorType.NotFound);

        var serviceApplication = await uow.ServiceApplications.GetByIdAsync(request.ServiceApplicationId.Value);
        if (serviceApplication == null)
            return Failure("Service application not found", ErrorType.NotFound);

        // Validate service application status
        if (serviceApplication.Status != ApplicationStatus.Completed)
            return Failure("Service application must be completed before enrollment", ErrorType.BadRequest);
        
        return Success;
    }
    
    public static async Task<Result> ValidateForCourseAsync(this PrerequisiteRequest request, IUnitOfWork uow)
    {
        if (request.CourseId == request.PrerequisiteCourseId)
            return Failure("Course cannot be prerequisite of itself", ErrorType.BadRequest);

        if (request.MinimumGrade is < 0 or > 100)
            return Failure("Minimum grade must be between 0 and 100", ErrorType.BadRequest);

        if (!await uow.Courses.DoesExistsAsync(request.CourseId.Value))
            return Failure("Specified course does not exist", ErrorType.NotFound);
        
        if (!await uow.Courses.DoesExistsAsync(request.PrerequisiteCourseId.Value))
            return Failure("Specified prerequisite course does not exist", ErrorType.NotFound);

        if (await uow.Prerequisites.DoesExistsAsync(request.CourseId.Value, request.PrerequisiteCourseId.Value))
            return Failure("Prerequisite already exists", ErrorType.Conflict);

        return Success;
    }
    
    public static async Task<Result> ValidateFinancialHoldRequestAsync(this FinancialHoldRequest request, IUnitOfWork uow)
    {
        if (request == default)
            return Result.Failure("Request cannot be null", ErrorType.BadRequest);

        if (string.IsNullOrWhiteSpace(request.Reason))
            return Result.Failure("Reason is required", ErrorType.BadRequest);

        if (request.HoldAmount is null or <= 0)
            return Result.Failure("Valid hold amount is required", ErrorType.BadRequest);

        if (!request.StudentId.HasValue || !await uow.Students.DoesExistsAsync(request.StudentId.Value))
            return Result.Failure("Valid student ID is required", ErrorType.BadRequest);

        if (!request.PlacedByUserId.HasValue || !await uow.Users.DoesExistAsync(request.PlacedByUserId.Value))
            return Result.Failure("Valid user ID is required", ErrorType.BadRequest);

        return Result.Success;
    }
}