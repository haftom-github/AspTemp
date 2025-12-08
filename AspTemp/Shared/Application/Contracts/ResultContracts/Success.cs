namespace AspTemp.Shared.Application.Contracts.ResultContracts;

public sealed class Success(
    string? message = null, 
    IDictionary<string, object>? details = null
) : Success<Unit>(new Unit(), message, details)
{
    public static implicit operator Success(string message)
        => new(message);
}

public class Success<T>(
    T value,
    string? message = null,
    IDictionary<string, object>? details = null)
{
    public T Value { get; } = value;
    public string Message { get; } = message ?? "Operation successful";
    public readonly IDictionary<string, object>? Details = details;

    public static implicit operator Success<T>(T value)
        => new(value);
}

public class PaginatedSuccess<T>(
    int pageNumber,
    int pageSize,
    int totalCount,
    IEnumerable<T> value,
    string? message = null,
    IDictionary<string, object>? details = null
) : Success<IEnumerable<T>>(value, message, details)
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public int TotalCount { get; } = totalCount;
}

public readonly record struct Unit;
