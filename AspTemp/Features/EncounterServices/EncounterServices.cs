using AspTemp.Features.Encounters;
using AspTemp.Features.MedicalServices;
using AspTemp.Shared.Domain;

namespace AspTemp.Features.EncounterServices;

public class EncounterService: AuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EncounterId { get; set; }
    public Encounter? Encounter { get; set; }
    
    public Guid MedicalServiceId { get; set; }
    public MedicalService? MedicalService { get; set; }
    public ServiceStatus Status { get; set; } = ServiceStatus.Ordered;
    
    public decimal? OutSourceFee { get; set; }
    public string? OutSourceNote { get; set; }
    
    public DateTime OrderedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? StartedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
}

public enum ServiceStatus
{
    Ordered = 1,
    InProgress = 2,
    Consumed = 3,
    Cancelled = 4,
    OutSourced = 5,
    OutSourceCompleted = 6
}
