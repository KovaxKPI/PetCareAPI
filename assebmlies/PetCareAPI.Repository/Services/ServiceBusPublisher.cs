using Azure.Messaging.ServiceBus;
using PetCareAPI.Application.Models;
using PetCareAPI.Application.Services;

namespace PetCareAPI.Repository.Services;

public class ServiceBusPublisher(ServiceBusClient client) : IMessagePublisher
{
    private const string QueueName = "appointment-queue";

    public async Task PublishAppointmentEventAsync(PendingMessage pendingMessage, CancellationToken cancellationToken)
    {
        await using var sender = client.CreateSender(QueueName);

        var serviceBusMessage = new ServiceBusMessage(pendingMessage.Payload)
        {
            MessageId = pendingMessage.MessageId.ToString(),
            Subject = pendingMessage.MessageType.ToString(),
        };

        await sender.SendMessageAsync(
            serviceBusMessage,
            cancellationToken);
    }
}