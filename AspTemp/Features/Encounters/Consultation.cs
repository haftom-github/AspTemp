using AspTemp.Features.Appointments;
using AspTemp.Features.EncounterServices;
using AspTemp.Features.MedicalServices;

namespace AspTemp.Features.Encounters;

public class Consultation: Encounter
{
    public string? Diagnosis { get; set; }
    public string? Notes { get; set; }
    public string? PatientComplaint { get; set; }
    
    public Consultation(Appointment appointment)
    {
        Services.Add(new EncounterService
        {
            EncounterId = Id,
            Encounter = this,
            MedicalService = appointment.Physician!,
            MedicalServiceId = appointment.Physician!.Id,
            Status = ServiceStatus.Consumed
        });
    }

    public void PrescribeMedication(Drug drug)
    {
        Services.Add(new Prescription(drug, this));
    }
}
