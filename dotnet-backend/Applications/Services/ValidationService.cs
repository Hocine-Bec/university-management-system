using Applications.Interfaces.Services;
using Applications.Shared;
using Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using static Applications.Shared.Result;

namespace Applications.Services;

public class ValidationService(IServiceProvider serviceProvider) : IValidationService
{
    private readonly IServiceProvider _service = serviceProvider;

    public async Task<Result> ValidateAsync<T>(T model)
    {
        var validator = _service.GetService<IValidator<T>>();
        if (validator == null) 
            return Success;

        var validationResult = await validator.ValidateAsync(model);
        if (validationResult.IsValid)
            return Success;

        var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
        return Failure(errors, ErrorType.BadRequest);
    }

    public Result Validate<T>(T model)
    {
        var validator = _service.GetService<IValidator<T>>();
        if (validator == null) 
            return Success;

        var validationResult = validator.Validate(model);
        if (validationResult.IsValid)
            return Success;

        var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
        return Failure(errors, ErrorType.BadRequest);
    }
}