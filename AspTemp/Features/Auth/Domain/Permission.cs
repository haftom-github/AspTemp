using AspTemp.Shared.Domain;

namespace AspTemp.Features.Auth.Domain;

public class Permission: AggregateRootBase<Guid>
{
    public string Name { get; private set; } = null!;
}