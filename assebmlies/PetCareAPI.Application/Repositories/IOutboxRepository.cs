using PetCareAPI.Application.Models;

namespace PetCareAPI.Application.Repositories;

public interface IOutboxRepository
{
    Task<IReadOnlyList<PendingMessage>> GetPendingMessagesAsync(CancellationToken cancellationToken);
    
    Task MarkMessageProcessedAsync(Guid messageId, CancellationToken cancellationToken);
}