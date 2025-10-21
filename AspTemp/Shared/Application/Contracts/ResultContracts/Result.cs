namespace AspTemp.Shared.Application.Contracts.ResultContracts;

public class Result
{
    public bool IsSuccess { get; }
    public string? Message { get; }
    public IReadOnlyList<Failure>? Failures { get; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    
    public IResult ToHttpResult()
    {
        if (!IsSuccess) return Results.Ok(this);
        var firstFailure = Failures?[0];
        return firstFailure?.Type switch
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
        IEnumerable<Failure>? failures = null
    )
    {
        IsSuccess = isSuccess;
        Message = message;
        Failures = failures?
            .ToList()
            .AsReadOnly();
    }
    
    public static Result Success(string? message = null) 
        => new(true, message);
    
    public static Result<T> Success<T>(T value, string? message = null) 
        => new(true, value, message);
    
    public static PaginatedResult<T> Success<T>(IEnumerable<T> value, int pageNumber, int pageSize, int totalCount, string? message = null) 
        => new(true, pageNumber, pageSize, totalCount, value, message);
    
    
    
    public static Result Failure(string? message = null, IEnumerable<Failure>? failures = null) 
        => new(false, message, failures);

    public static Result<T> Failure<T>(string? message = null, IEnumerable<Failure>? failures = null) 
        => new(false, default, message, failures);
    
    public static PaginatedResult<T> FailurePaginated<T>(string? message = null, IEnumerable<Failure>? failures = null)
        => new(false, failures:failures, message:message);

}

public class Result<T> : Result
{
    private readonly T? _value;
    
    public T? Value => !IsSuccess 
        ? throw new InvalidOperationException("Cannot access Value of a failed result") 
        : _value;

    protected internal Result(bool isSuccess, T? value = default, string? message = null, IEnumerable<Failure>? failures = null)
        : base(isSuccess, message, failures)
    {
        _value = value;
    }
}


public class PaginatedResult<T> : Result<IReadOnlyList<T>>
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
        IEnumerable<Failure>? failures = null)
        : base(isSuccess, value?.ToList().AsReadOnly(), message, failures)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}