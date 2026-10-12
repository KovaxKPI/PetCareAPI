namespace PetCareAPI.Contracts.Events;

public record AppointmentUpdatedEvent(Guid AppointmentId, string PetName, DateTimeOffset AppointmentDate, string GooglePlaceId, string UserEmail);