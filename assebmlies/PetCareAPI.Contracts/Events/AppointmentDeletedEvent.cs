namespace PetCareAPI.Contracts.Events;

public record AppointmentDeletedEvent(Guid AppointmentId, string PetName, DateTimeOffset AppointmentDate, string UserEmail);