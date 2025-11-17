namespace AspTemp.Shared.Application.Contracts.ResultContracts;

public class Result
{
    public bool IsSuccess { get; }
    public string? Message { get; }
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

    protected Result(
        bool isSuccess, 
        string? message = null, 
        Failure? failure = null
    ){
        IsSuccess = isSuccess;
        Message = message;
        Failure = failure;
    }
    
    public static Result Succeed(string? message = null) 
        => new(true, message);
    
    public static Result Fail(Failure failure, string? message = null)
        => new(false, message, failure);
    
    public static Result<T> Succeed<T>(T value, string? message = null) 
        => new(true, value, message);
    
    public static Result<T> Fail<T>(Failure failure, string? message = null)
        => new(false, failure:failure, message:message);
    
    public static PaginatedResult<T> Succeed<T>(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount, string? message = null)
        => new(true, pageNumber,pageSize,totalCount,items, message);
    
    public static PaginatedResult<T> FailPaginated<T>(Failure failure, string? message = null)
        => new(false, failure:failure, message:message);
    
    public static implicit operator Result(Failure failure)
        => Fail(failure);
}

public class Result<T> : Result
{
    private readonly T? _value;
    
    public T? Value => !IsSuccess 
        ? throw new InvalidOperationException("Cannot access Value of a failed result") 
        : _value;

    protected internal Result(bool isSuccess, T? value = default, string? message = null, Failure? failure = null)
        : base(isSuccess, message, failure)
    {
        _value = value;
    }
    
    public static implicit operator Result<T>(T value)
        => Succeed(value);
    
    public static implicit operator Result<T>(Failure failure)
        => Fail<T>(failure);
}


public class PaginatedResult<T> : Result<IEnumerable<T>>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    
    protected internal PaginatedResult(
        bool isSuccess,
        int pageNumber = 0,
        int pageSize = 0,
        int totalCount = 0,
        IEnumerable<T>? value = null,
        string? message = null,
        Failure? failure = null)
        : base(isSuccess, value, message, failure)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
    
    public static implicit operator PaginatedResult<T>(Failure failure)
        => FailPaginated<T>(failure);
}