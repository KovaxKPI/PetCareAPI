using Microsoft.EntityFrameworkCore;
using PetCareAPI.Application.Models;
using PetCareAPI.Application.Repositories;

namespace PetCareAPI.Repository.Persistent;

public class OutboxRepository(AppDbContext context) : IOutboxRepository
{
    public async Task<IReadOnlyList<PendingMessage>> GetPendingMessagesAsync(CancellationToken cancellationToken)
    {
        return await context.OutboxMessages.AsNoTracking().Where(x => x.ProcessedAt == null)
            .Select(x => new PendingMessage(x.MessageId, x.Type, x.Payload)).ToListAsync(cancellationToken);
    }

    public async Task MarkMessageProcessedAsync(Guid messageId, CancellationToken cancellationToken)
    {
        await context.OutboxMessages.Where(x => x.MessageId == messageId)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.ProcessedAt, DateTimeOffset.Now), cancellationToken);
    }
}