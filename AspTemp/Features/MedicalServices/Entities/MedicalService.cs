using AspTemp.Shared.Domain;

namespace AspTemp.Features.MedicalServices.Entities;

public class MedicalService: AuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public List<MedicalServiceFee> Fees { get; set; } = [];
}