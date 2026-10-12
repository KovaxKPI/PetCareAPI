using PetCareAPI.Contracts.Enums;

namespace PetCareAPI.Repository.Outbox;

public class OutboxMessage
{
    public Guid MessageId { get; } = Guid.CreateVersion7();

    public MessageType Type { get; set; }

    public string Payload { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ProcessedAt { get; set; }

    public string? Error { get; set; }

    public int RetryCount { get; set; }
}