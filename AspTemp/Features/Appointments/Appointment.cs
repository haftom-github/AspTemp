using AspTemp.Features.Encounters;
using AspTemp.Features.MedicalServices;

namespace AspTemp.Features.Appointments;

public class Appointment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PhysicianId { get; set; }
    public Physician? Physician { get; set; }
    
    public List<Encounter> Encounters { get; set; } = [];
}
