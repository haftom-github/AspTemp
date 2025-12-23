using Microsoft.AspNetCore.Mvc;

namespace AspTemp.Shared.Application.Contracts.ResultContracts.Extensions;

public static class FailureExtensions
{
    public static ProblemDetails ToProblemDetails(this Failure failure)
    {
        return new ProblemDetails
        {
            Type = GetType(failure.FailureType),
            Title = GetTitle(failure.FailureType),
            Detail = failure.Message,
            Status = GetStatus(failure.FailureType),
            Extensions = new Dictionary<string, object?>{{"details", failure.Details}}
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

    public static int GetStatus(this FailureType failureType)
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
