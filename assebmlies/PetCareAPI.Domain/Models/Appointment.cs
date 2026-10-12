namespace PetCareAPI.Domain.Models;

public class Appointment
{
    public Guid AppointmentId { get; } = Guid.CreateVersion7();

    public Guid PetId { get; set; }
    
    public DateTimeOffset AppointmentDate { get; set; }

    public string GooglePlaceId { get; set; } = null!;

    public void Update(DateTimeOffset appointmentDate, string googlePlaceId)
    {
        AppointmentDate = appointmentDate;
        GooglePlaceId = googlePlaceId;
    }
}