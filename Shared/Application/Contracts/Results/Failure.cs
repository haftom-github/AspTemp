namespace Shared.Application.Contracts.Results;

public class Failure
{
    public string Message { get; }
    public FailureType Type { get; }
    public IReadOnlyDictionary<string, object>? Metadata { get; }
    
    protected Failure(string message, FailureType type, Dictionary<string, object>? metadata = null)
    {
        Message = message;
        Type = type;
        Metadata = metadata;
    }
    
    public static ValidationFailure Validation(string message, string key) => new(message, key);
    public static Failure NotFound(string message = "resource not found")
        => new(message, FailureType.NotFound);
    
    public static Failure Unauthorized(string message = "unauthorized access")
        => new(message, FailureType.Unauthorized);
    
    public static Failure Conflict(string message = "conflict occured", Dictionary<string, object>? metadata = null)
        => new(message, FailureType.Conflict, metadata);
    
    public static Failure InvalidOperation(string message = "invalid operation")
        => new(message, FailureType.InvalidOperation);
    
    public static Failure Unexpected(string message = "unexpected error occured")
        => new(message, FailureType.Unexpected);
    
    public static Failure WithMetadata(string message, FailureType type, Dictionary<string, object> metadata)
        => new(message, type, metadata);
}

public class ValidationFailure : Failure
{
    public string PropertyName { get; }
    
    protected internal ValidationFailure(string message, string propertyName)
        : base(message, FailureType.Validation)
    {
        PropertyName = propertyName;
    }
}

public enum FailureType
{
    Validation = 1,
    NotFound = 2,
    Unauthorized = 3,
    Conflict = 4,
    InvalidOperation = 5,
    Unexpected = 6
}
