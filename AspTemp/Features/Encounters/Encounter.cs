using AspTemp.Features.Appointments;
using AspTemp.Features.EncounterServices;

namespace AspTemp.Features.Encounters;

public class Encounter
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
    
    public List<EncounterService> Services { get; set; } = [];
    public DateTime EncounterDate { get; set; } = DateTime.UtcNow;
}