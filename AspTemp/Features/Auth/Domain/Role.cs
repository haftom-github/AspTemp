using AspTemp.Shared.Domain;

namespace AspTemp.Features.Auth.Domain;

public class Role: AggregateRootBase<Guid>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    
    public Guid? ParentId { get; private set; }
    public Role? Parent { get; private set; }

    private readonly List<Role> _roles = [];
    public IEnumerable<Role> Roles => _roles;
    
    private readonly List<Permission> _permissions = [];
    public IEnumerable<Permission> Permissions => _permissions;

    public void AddChildren(IEnumerable<Role> roles)
    {
        _roles.AddRange(roles
            .DistinctBy(r => r.Id)
            .Where(r => _roles.All(s => r.Id != s.Id))
        );
    }

    public void RemoveChildren(IEnumerable<Role> roles)
    {
        _roles.RemoveAll(r => roles.Any(rtr => rtr.Id == r.Id));
    }

    public void AddPermissions(IEnumerable<Permission> permissions)
    {
        _permissions.AddRange(permissions
            .DistinctBy(p => p.Id)
            .Where(p => _permissions.All(pta => p.Id != pta.Id))
        );
    }

    public void RemovePermissions(IEnumerable<Permission> permissions)
    {
        _permissions.RemoveAll(p => permissions.Any(ptr => p.Id == ptr.Id));
    }
}
