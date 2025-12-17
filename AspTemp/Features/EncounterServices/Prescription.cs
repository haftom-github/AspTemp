using AspTemp.Features.Encounters;
using AspTemp.Features.MedicalServices;

namespace AspTemp.Features.EncounterServices;

public class Prescription: EncounterService
{
    public Prescription(Drug drug, Consultation consultation)
    {
        MedicalService = drug;
        MedicalServiceId = drug.Id;
        Encounter = consultation;
        EncounterId = consultation.Id;
        Status = ServiceStatus.Ordered;
    }
}
