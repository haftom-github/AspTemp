using AspTemp.Shared.Domain;

namespace AspTemp.Features.Auth.Domain;

public class User: AggregateRootBase<Guid>
{
    public string? Email { get; set; }

    private readonly List<AuthIdentity> _authIdentities = [];
    public IEnumerable<AuthIdentity> AuthIdentities 
        => _authIdentities;
    
    public AuthIdentity? LocalAuthIdentity 
        => _authIdentities.FirstOrDefault(ai => ai.AuthProvider?.Name == "local");
    
    // private readonly List<Role> _roles = [];
    // public IEnumerable<Role> Roles 
    //     => _roles;
    //
    // private readonly List<Permission> _plusPermissions = [];
    // public IEnumerable<Permission> PlusPermissions 
    //     => _plusPermissions;
    //
    // private readonly List<Permission> _minusPermissions = [];
    // public IEnumerable<Permission> MinusPermissions 
    //     => _minusPermissions;
    //
    // public void AssignRoles(IEnumerable<Role> rolesToAssign)
    // {
    //     _roles.AddRange(rolesToAssign
    //         .Distinct()
    //         .Where(rta => _roles.All(r => r.Id != rta.Id))
    //     );
    // }
    //
    // public void UnAssignRoles(IEnumerable<Role> rolesToUnAssign)
    // {
    //     _roles.RemoveAll(r => rolesToUnAssign.Any(rtu => rtu.Id == r.Id));
    // }

    public void AddAuthIdentity(Guid authProviderId, string providerUserId, string? password = null)
    {
        _authIdentities.Add(new AuthIdentity
        {
            UserId = Id,
            AuthProviderId = authProviderId,
            ProviderUserId = providerUserId,
            Password = password
        });
    }
}