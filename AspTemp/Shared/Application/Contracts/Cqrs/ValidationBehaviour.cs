using System.Reflection;
using AspTemp.Shared.Application.Contracts.ResultContracts;
using FluentValidation;
using MediatR;

namespace AspTemp.Shared.Application.Contracts.Cqrs;

public class ValidationBehaviour<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators) 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest: notnull
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResult = await Task.WhenAll(
            validators.Select(
                v => v.ValidateAsync(context, cancellationToken)
            )
        );

        var failures = validationResult
            .Where(vr => vr.Errors.Count != 0)
            .SelectMany(v => v.Errors)
            .Where(e => e is not null)
            .Select(e => (e.PropertyName, e.ErrorMessage))
            .ToArray();

        if (failures.Length == 0) return await next(cancellationToken);
        
        var valueType = typeof(TResponse).GetGenericArguments()[0];

        var failureResult = typeof(Result<>)
            .MakeGenericType(valueType)
            .GetConstructor(
                BindingFlags.Public | BindingFlags.Instance,
                [typeof(Failure)])!
            .Invoke([Failure.Validation(failures)]);

        return (TResponse)failureResult;
    }
}
