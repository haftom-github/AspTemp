using AspTemp.Shared.Domain;

namespace AspTemp.Features.MedicalServices;

public class MedicalServiceFee: AuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid MedicalServiceId { get; set; }
    
    public Guid? EmployeeCategoryId { get; set; }
    
    public decimal Amount { get; set; }
    
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    
    public bool IsEffectiveOn(DateTime? date)
    {
        date ??= DateTime.UtcNow;
        return EffectiveFrom <= date && date <= (EffectiveTo ?? DateTime.MaxValue);
    }
}