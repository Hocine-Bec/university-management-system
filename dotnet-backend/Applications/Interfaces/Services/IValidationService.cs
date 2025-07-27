using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface IValidationService
{
    Task<Result> ValidateAsync<T>(T model);
    Result Validate<T>(T model);
}