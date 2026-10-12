using PetCareAPI.Application.Models;

namespace PetCareAPI.Application.Services;

public interface IMessagePublisher
{
    Task PublishAppointmentEventAsync(PendingMessage pendingMessage, CancellationToken cancellationToken);
}