namespace AspTemp.Shared.Application.Contracts.ResultContracts;

public record Failure(string Message, FailureType FailureType, IReadOnlyDictionary<string, object>? Metadata = null)
{
    public static ValidationFailure Validation(Dictionary<string, string> failures, string message = "one or more validation failures occurred") 
        => new(message, failures);
    
    public static ValidationFailure Validation(string propertyName, string validationMessage)
        => Validation(new Dictionary<string, string> { { propertyName, validationMessage } });
    
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

public record ValidationFailure(
    string Message,
    IReadOnlyDictionary<string, string> Failures,
    IReadOnlyDictionary<string, object>? Metadata = null) 
    : Failure(Message, FailureType.Validation, Metadata);

public enum FailureType
{
    Validation = 1,
    NotFound = 2,
    Unauthorized = 3,
    Conflict = 4,
    InvalidOperation = 5,
    Unexpected = 6
}
