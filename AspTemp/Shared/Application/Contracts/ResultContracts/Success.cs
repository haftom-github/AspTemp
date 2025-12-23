namespace AspTemp.Shared.Application.Contracts.ResultContracts;

public class Success<T>(
    T value,
    string? message = null,
    IDictionary<string, object>? details = null)
{
    public T Value { get; } = value;
    public string Message { get; } = message ?? "Operation successful";
    public readonly IDictionary<string, object>? Details = details;
}

public class PaginatedSuccess<T>(
    IEnumerable<T> value,
    int pageNumber,
    int pageSize,
    int totalCount,
    string? message = null,
    IDictionary<string, object>? details = null
) : Success<IEnumerable<T>>(value, message, details)
{
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
    public int TotalCount { get; } = totalCount;
}
