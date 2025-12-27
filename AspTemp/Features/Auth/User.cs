using AspTemp.Shared.Domain;

namespace AspTemp.Features.Auth;

public class User: AggregateRootBase<Guid>
{
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string Password { get; set; } = null!;
}