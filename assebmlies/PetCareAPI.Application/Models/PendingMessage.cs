using PetCareAPI.Contracts.Enums;

namespace PetCareAPI.Application.Models;

public record PendingMessage(Guid MessageId, MessageType MessageType, string Payload);