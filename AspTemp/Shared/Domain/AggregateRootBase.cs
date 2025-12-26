namespace AspTemp.Shared.Domain;

public abstract class AggregateRootBase<TId>
{
    public TId Id { get; protected set; } = default!;
    
    private readonly List<IDomainEvent> _domainEvents = [];
    public IEnumerable<IDomainEvent> DomainEvents => _domainEvents;
    
    protected void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
    
    public RecordStatus RecordStatus { get; protected set; } = RecordStatus.Active;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
}

public enum RecordStatus
{
    Inactive = 1,
    Active = 2,
    Deleted = 3
}
