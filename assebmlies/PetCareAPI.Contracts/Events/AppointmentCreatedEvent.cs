namespace PetCareAPI.Contracts.Events;

public record AppointmentCreatedEvent(Guid AppointmentId, string PetName, DateTimeOffset AppointmentDate, string GooglePlaceId, string UserEmail);