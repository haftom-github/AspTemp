using AspTemp.Shared.Domain;

namespace AspTemp.Features.Auth.Domain;

public class AuthProvider : AggregateRootBase<Guid>
{
    public string Name { get; set; } = null!;
    public string? ClientId { get; set; }
}