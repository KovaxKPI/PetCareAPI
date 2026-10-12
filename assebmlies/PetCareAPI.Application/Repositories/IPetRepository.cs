using PetCareAPI.Contracts.Events;
using PetCareAPI.Domain.Models;

namespace PetCareAPI.Application.Repositories;

public interface IPetRepository
{
    Task<List<Pet>> GetAllPets(string userId, CancellationToken cancellationToken);
    
    Task<Pet?> GetPetById(Guid petId, CancellationToken cancellationToken);
    
    Task<Appointment?> GetAppointmentById(Guid appointmentId, CancellationToken cancellationToken);
    
    Task AddPetAsync(Pet pet, CancellationToken cancellationToken);
    
    Task AddAppointmentAsync(Appointment appointment, AppointmentCreatedEvent appointmentCreatedEvent, CancellationToken cancellationToken);
    
    Task DeleteAppointmentAsync(Appointment appointment, AppointmentDeletedEvent appointmentDeletedEvent, CancellationToken cancellationToken);

    Task UpdateAppointmentAsync(AppointmentUpdatedEvent appointmentUpdatedEvent, CancellationToken cancellationToken);
}