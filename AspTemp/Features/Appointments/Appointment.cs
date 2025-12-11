using AspTemp.Features.MedicalServices.Entities;

namespace AspTemp.Features.Appointments;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public List<Encounter> Encounters { get; set; } = [];
}