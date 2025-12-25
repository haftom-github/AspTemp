namespace AspTemp.Shared.Domain;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}