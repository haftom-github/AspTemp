using Microsoft.AspNetCore.Mvc;

namespace AspTemp.Shared.Application.Contracts.ResultContracts;

public class Result<T>
{
    public bool IsSuccess => Success != null;
    public Success<T>? Success { get; }
    public Failure? Failure { get; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    
    public Result(
        Success<T> success
    ){
        Success = success;
        Failure = null;
    }

    public Result(Failure failure)
    {
        Failure = failure;
        Success = null;
    }
    
    public static implicit operator Result<T>(Failure failure)
        => new(failure);

    public static implicit operator Result<T>(Success<T> success)
        => new(success);
}

public sealed class Result(Success success) : Result<Unit>(success);

public static class ResultExtensions
{
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper)
        => result.IsSuccess
            ? new Success<TOut>(
                mapper(result.Success!.Value), 
                result.Success.Message, 
                result.Success.Details!.ToDictionary()
            )
            : result.Failure!;

    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> mapper)
        => result.IsSuccess
            ? new Success<TOut>(
                await mapper(result.Success!.Value),
                result.Success.Message,
                result.Success.Details?.ToDictionary()
            )
            : result.Failure!;

    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, Func<TIn, Result<TOut>> binder)
        => result.IsSuccess
            ? binder(result.Success!.Value)
            : result.Failure!;

    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> binder)
        => result.IsSuccess
            ? await binder(result.Success!.Value)
            : result.Failure!;

    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        return result.IsSuccess 
            ? Results.Json(result.Success, statusCode:200) 
            : Results.Json(result.Failure!.ToProblemDetails(), statusCode: GetStatus(result.Failure!.FailureType));
    }

    private static ProblemDetails ToProblemDetails(this Failure failure)
    {
        return new ProblemDetails
        {
            Type = GetType(failure.FailureType),
            Title = GetTitle(failure.FailureType),
            Detail = failure.Message,
            Status = GetStatus(failure.FailureType),
            Extensions = failure.Details
        };
    }
    
    private static string GetType(FailureType failureType)
    {
        return failureType switch
        {
            FailureType.NotFound => "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4",
            FailureType.Unauthorized => "https://www.rfc-editor.org/rfc/rfc7235#section-3.1",
            FailureType.Validation => "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.1",
            FailureType.Conflict => "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.8",
            FailureType.Forbidden => "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.3",
            FailureType.Unexpected => "https://www.rfc-editor.org/rfc/rfc7231#section-6.6.1",
            _ => throw new ArgumentOutOfRangeException(nameof(failureType), failureType, null)
        };
    }

    private static string GetTitle(FailureType failureType)
    {
        return failureType switch
        {
            FailureType.Validation => "One Or More Validation Errors Occured",
            FailureType.NotFound => "Resource Not Found",
            FailureType.Unauthorized => "Unauthorized Access",
            FailureType.Conflict => "Conflict Occured",
            FailureType.Forbidden => "Action Forbidden",
            FailureType.Unexpected => "Internal Server Error",
            _ => throw new ArgumentOutOfRangeException(nameof(failureType), failureType, null)
        };
    }

    private static int GetStatus(FailureType failureType)
    {
        return failureType switch
        {
            FailureType.Validation => 400,
            FailureType.NotFound => 404,
            FailureType.Unauthorized => 401,
            FailureType.Conflict => 409,
            FailureType.Forbidden => 403,
            FailureType.Unexpected => 500,
            _ => throw new ArgumentOutOfRangeException(nameof(failureType), failureType, null)
        };
    }
}
