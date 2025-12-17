using AspTemp.Shared.Domain;

namespace AspTemp.Features.MedicalServices;

public class MedicalService: AuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public List<MedicalServiceFee> Fees { get; set; } = [];
    public ServiceType ServiceType { get; set; } = ServiceType.PerConsumption;
}

public enum ServiceType
{
    PerConsumption = 1,
    PerDay = 2,
    PerHour = 3
}
