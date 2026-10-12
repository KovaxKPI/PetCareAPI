using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PetCareAPI.Application.Repositories;
using PetCareAPI.Contracts.Enums;
using PetCareAPI.Contracts.Events;
using PetCareAPI.Domain.Models;
using PetCareAPI.Repository.Outbox;

namespace PetCareAPI.Repository.Persistent;

public class PetRepository(AppDbContext context) : IPetRepository
{
    public async Task<List<Pet>> GetAllPets(string userId, CancellationToken cancellationToken)
    {
        return await context.Pets.Where(x => x.UserId == userId).Include(x => x.Appointments).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Pet?> GetPetById(Guid petId, CancellationToken cancellationToken)
    {
        return await context.Pets.FirstOrDefaultAsync(x => x.PetId == petId, cancellationToken);
    }

    public async Task<Appointment?> GetAppointmentById(Guid appointmentId, CancellationToken cancellationToken)
    {
        return await context.Appointments.FirstOrDefaultAsync(x => x.AppointmentId == appointmentId, cancellationToken: cancellationToken);
    }

    public async Task AddPetAsync(Pet pet, CancellationToken cancellationToken)
    {
        context.Pets.Add(pet);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAppointmentAsync(Appointment appointment, AppointmentCreatedEvent appointmentCreatedEvent, CancellationToken cancellationToken)
    {
        context.Appointments.Add(appointment);
        
        var message = new OutboxMessage
        {
            Type = MessageType.AppointmentCreated,
            CreatedAt = DateTimeOffset.Now,
            Payload = JsonSerializer.Serialize(appointmentCreatedEvent)
        };
        
        context.OutboxMessages.Add(message);
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAppointmentAsync(Appointment appointment, AppointmentDeletedEvent appointmentDeletedEvent, CancellationToken cancellationToken)
    {
        context.Appointments.Remove(appointment);

        context.OutboxMessages.Add(new OutboxMessage
        {
            Type = MessageType.AppointmentDeleted,
            CreatedAt = DateTimeOffset.UtcNow,
            Payload = JsonSerializer.Serialize(appointmentDeletedEvent)
        });

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAppointmentAsync(AppointmentUpdatedEvent appointmentUpdatedEvent, CancellationToken cancellationToken)
    {
        context.OutboxMessages.Add(new OutboxMessage
        {
            Type = MessageType.AppointmentUpdated, 
            CreatedAt = DateTimeOffset.UtcNow, 
            Payload = JsonSerializer.Serialize(appointmentUpdatedEvent)
        });

        await context.SaveChangesAsync(cancellationToken);
    }
}