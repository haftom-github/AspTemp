namespace AspTemp.Shared.Application.Contracts.ResultContracts;

public class Result<T>
{
    private bool IsSuccess => Success != null;
    public string Message => IsSuccess ? Success!.Message : Failure!.Message;
    public Success<T>? Success { get; }
    public Failure? Failure { get; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    
    public IResult ToHttpResult()
    {
        if (IsSuccess) return Results.Ok(this);
        return Failure!.FailureType switch
        {
            FailureType.NotFound => Results.NotFound(this),
            FailureType.Conflict => Results.BadRequest(this),
            FailureType.Unauthorized => Results.Unauthorized(),
            FailureType.InvalidOperation => Results.BadRequest(this),
            FailureType.Validation => Results.BadRequest(this),
            FailureType.Unexpected => Results.InternalServerError(this),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

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