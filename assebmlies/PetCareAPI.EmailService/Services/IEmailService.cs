using Azure.Messaging.ServiceBus;

namespace PetCareAPI.EmailService.Services;

public interface IEmailService
{
    Task SendEmailAsync(ServiceBusReceivedMessage outboxMessage, CancellationToken stoppingToken);
}