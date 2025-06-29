using Applications.DTOs.Semester;
using Applications.Shared;
using Domain.Enums;

namespace Applications.Helpers;

public static class SemesterTermExtension
{
    /// <summary>
    /// Generates a standardized term code by combining the first two letters of
    /// the term and the last two-digit year. (e.g., "Fall" + 2023 → "FA23").
    /// </summary>
    /// <param name="request"></param>
    /// <returns>A 4-character term code</returns>
    public static Result<string> GenerateTermCode(this SemesterRequest request)
    {
        if (string.IsNullOrEmpty(request.Term) || request.Term.Length < 2)
            return Result<string>.Failure("Term must be at least 2 characters long.", ErrorType.BadRequest);

        if (request.Year is < 2000 or > 2100)
            return Result<string>.Failure("Year must be between 2000 and 2100.", ErrorType.BadRequest);
        
        string termPrefix = request.Term[..2].ToUpper();
        int shortYear = request.Year.Value % 100;
        return Result<string>.Success($"{termPrefix}{shortYear:D2}");
    }
}