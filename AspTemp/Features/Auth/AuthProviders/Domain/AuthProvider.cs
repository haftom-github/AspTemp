using AspTemp.Shared.Domain;

namespace AspTemp.Features.Auth.AuthProviders.Domain;

public class AuthProvider : AggregateRootBase<Guid>
{
    public string Name { get; set; } = null!;
    public string? ClientId { get; set; }

    public static Guid LocalAuthProviderId => Guid.Parse("00000000-0000-0000-0001-000000000001");
}