using AspTemp.Shared.Domain;

namespace AspTemp.Features.Auth.Domain;

public class User: AggregateRootBase<Guid>
{
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string Password { get; set; } = null!;
    
    private readonly List<Role> _roles = [];
    public IEnumerable<Role> Roles => _roles;

    private readonly List<Permission> _plusPermissions = [];
    public IEnumerable<Permission> PlusPermissions => _plusPermissions;
    
    private readonly List<Permission> _minusPermissions = [];
    public IEnumerable<Permission> MinusPermissions => _minusPermissions;

    public void AssignRoles(IEnumerable<Role> rolesToAssign)
    {
        _roles.AddRange(rolesToAssign
            .Distinct()
            .Where(rta => _roles.All(r => r.Id != rta.Id))
        );
    }

    public void UnAssignRoles(IEnumerable<Role> rolesToUnAssign)
    {
        _roles.RemoveAll(r => rolesToUnAssign.Any(rtu => rtu.Id == r.Id));
    }
}