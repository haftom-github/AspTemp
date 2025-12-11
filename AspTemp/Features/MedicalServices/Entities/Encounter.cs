using AspTemp.Features.Appointments;

namespace AspTemp.Features.MedicalServices.Entities;

public class Encounter
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }
}