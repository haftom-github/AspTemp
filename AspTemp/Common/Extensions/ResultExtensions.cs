using Shared.Application.Contracts.Results;

namespace AspTemp.Common.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult(this Result result)
    {
        if (!result.IsSuccess)
        {
            var firstFailure = result.Failures?[0];
            return firstFailure?.Type switch
            {
                FailureType.Validation => Results.BadRequest(result),
                FailureType.NotFound => Results.NotFound(result),
                FailureType.Unauthorized => Results.Unauthorized(),
                FailureType.Conflict => Results.Conflict(result),
                FailureType.InvalidOperation => Results.BadRequest(result),
                FailureType.Unexpected => Results.InternalServerError(result),
                _ => Results.BadRequest(result)
            };
        }

        return Results.Ok();
    }
}