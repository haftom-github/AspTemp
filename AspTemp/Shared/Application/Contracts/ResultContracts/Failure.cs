namespace AspTemp.Shared.Application.Contracts.ResultContracts;

public class Failure
{
    public string Message { get; }

    public FailureType FailureType { get; }
    public IReadOnlyDictionary<string, object>? Metadata { get; }

    protected Failure(string message, FailureType failureType, IDictionary<string, object>? metadata = null)
    {
        Message = message;
        FailureType = failureType;
        Metadata = metadata?.AsReadOnly();
    }
    
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

public class ValidationFailure
    : Failure
{
    public IReadOnlyDictionary<string, string> Failures;
    protected internal ValidationFailure(
        string message,
        IDictionary<string, string> failures,
        IDictionary<string, object>? metadata = null
    ) : base(message, FailureType.Validation, metadata)
    {
        Failures = failures.AsReadOnly();
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
