using AspTemp.Features.Auth.AuthProviders.Domain;

namespace AspTemp.Features.Auth.Users.Domain;

public class AuthIdentity
{
    public Guid UserId { get; set; }
    public Guid AuthProviderId { get; set; }
    public AuthProvider? AuthProvider { get; set; }
    
    public string ProviderUserId { get; set; } = null!;
    public string? Password { get; set; }
}