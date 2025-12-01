namespace AspTemp.Shared.Application.Contracts.ResultContracts;

public sealed class Success(
    string? message = null, 
    IDictionary<string, object>? metadata = null
) : Success<Unit>(new Unit(), message, metadata)
{
    public static implicit operator Success(string message)
        => new(message);
}

public class Success<T>(
    T value,
    string? message = null,
    IDictionary<string, object>? metadata = null)
{
    public T Value { get; } = value;
    public string Message { get; } = message ?? "Operation successful";
    public IReadOnlyDictionary<string, object>? Metadata = metadata?.AsReadOnly();

    public static implicit operator Success<T>(T value)
        => new(value);
}

public class PaginatedSuccess<T>(
    int pageNumber,
    int pageSize,
    int totalCount,
    IEnumerable<T> value,
    string? message = null,
    IDictionary<string, object>? metadata = null
) : Success<IEnumerable<T>>(value, message, metadata)
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public int TotalCount { get; } = totalCount;
}

public readonly record struct Unit;
