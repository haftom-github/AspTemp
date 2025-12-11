using AspTemp.Features.MedicalServices.Entities;

namespace AspTemp.Features.Physicians;

public class Physician: MedicalService
{
    public string FullName { get; set; } = string.Empty;
}