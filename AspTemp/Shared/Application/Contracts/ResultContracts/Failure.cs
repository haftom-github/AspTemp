namespace AspTemp.Shared.Application.Contracts.ResultContracts;

public class Failure
{
    public string Message { get; }

    public FailureType FailureType { get; }
    public IDictionary<string, object?> Details { get; }

    private Failure(string message, FailureType failureType, IDictionary<string, object?>? detail = null)
    {
        Message = message;
        FailureType = failureType;
        Details = detail ?? new Dictionary<string, object?>();
    }
    
    public static Failure NotFound(string message = "resource not found", IDictionary<string, object?>? detail = null)
        => new(message, FailureType.NotFound, detail);
    
    public static Failure Unauthorized(string message = "unauthorized access")
        => new(message, FailureType.Unauthorized);
    
    public static Failure Conflict(string message)
        => new(message, FailureType.Conflict);
    
    public static Failure Conflict(
        string message, 
        string key, 
        string value)
        => new(message, FailureType.Conflict, new Dictionary<string, object?> { [key] = value });
    
    public static Failure Conflict(string key, string value)
        => new("Conflict", FailureType.Conflict, new Dictionary<string, object?> { [key] = value });
    
    public static Failure Validation(string message)
        => new(message, FailureType.Validation);
    
    public static Failure Validation(string key, string value)
        => new("One or more validation errors occured", FailureType.Validation, new Dictionary<string, object?> { [key] = value });
    
    public static Failure Validation(params (string Key, string Value)[] errors)
    {
        var details = errors.ToDictionary(
            e => e.Key, object? (e) => e.Value);
    
        return new Failure("One or more validation errors occurred", FailureType.Validation, details);
    }
    
    public static Failure InvalidOperation(string message = "invalid operation")
        => new(message, FailureType.Forbidden);
    
    public static Failure InvalidOperation(params (string Key, string Value)[] details)
        => new(
            "this operation is invalid", 
            FailureType.Forbidden, 
            details.ToDictionary(e => e.Key, object? (e) => e.Value)
        );
    
    public static Failure Unexpected()
        => new("Unexpected error occured", FailureType.Unexpected);
}

public enum FailureType
{
    Validation = 1,
    NotFound = 2,
    Unauthorized = 3,
    Conflict = 4,
    Forbidden = 5,
    Unexpected = 6
}
