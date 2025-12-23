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

    public static implicit operator Result<T>(T value)
        => new(new Success<T>(value));
}