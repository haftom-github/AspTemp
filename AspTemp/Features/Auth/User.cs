using AspTemp.Shared.Domain;

namespace AspTemp.Features.Auth;

public class User: AggregateRootBase<Guid, Guid>
{
    public string Firstname { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string Lastname { get; set; } = null!;
    
    public string? Email { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}