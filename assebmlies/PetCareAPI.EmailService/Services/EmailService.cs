using System.Text.Json;
using Azure.Messaging.ServiceBus;
using PetCareAPI.Contracts.Enums;
using PetCareAPI.Contracts.Events;
using Resend;

namespace PetCareAPI.EmailService.Services;

public class EmailService(ILogger<EmailService> logger, IConfiguration configuration) : IEmailService
{
    private const string DateFormat = "dddd, dd MMM yyyy h:mm";
    private const string GoogleMapsApiBaseUrl = "https://www.google.com/maps/search/?api=1&query=Google&query_place_id=";
    
    public async Task SendEmailAsync(ServiceBusReceivedMessage outboxMessage, CancellationToken stoppingToken)
    {
        try
        {
            var client = ResendClient.Create(new ResendClientOptions()
            {
                ApiToken = configuration["Resend:ApiToken"] ?? "",
            });
            var emailMessage = new EmailMessage
            {
                From = configuration["Resend:EmailSender"] ?? "",
            };
            
            if(!Enum.TryParse(outboxMessage.Subject, out MessageType messageType))
                throw new InvalidOperationException();
            
            switch (messageType)
            {
                case MessageType.AppointmentCreated:
                    await SendAppointmentCreatedEmailAsync(client, emailMessage, outboxMessage, stoppingToken);
                    break;
                case MessageType.AppointmentUpdated:
                    await SendAppointmentUpdatedEmailAsync(client, emailMessage, outboxMessage, stoppingToken);
                    break;
                case MessageType.AppointmentDeleted:
                    await SendAppointmentDeletedEmailAsync(client, emailMessage, outboxMessage, stoppingToken);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error sending email");
            throw;
        }
    }

    private static async Task SendAppointmentCreatedEmailAsync(IResend client, EmailMessage emailMessage, ServiceBusReceivedMessage message, CancellationToken stoppingToken)
    {
        var eventBody = JsonSerializer.Deserialize<AppointmentCreatedEvent>(message.Body.ToString());
        emailMessage.To.Add(eventBody?.UserEmail ?? throw new InvalidOperationException());
        emailMessage.Subject = $"Appointment for Pet {eventBody.PetName} has been created";
        
        var mapsUrl =  GoogleMapsApiBaseUrl + eventBody.GooglePlaceId;
        
        var html = $"""
                    <p>
                        Date: {eventBody.AppointmentDate.ToString(DateFormat)}
                    </p>

                    <p>
                        <a href="{mapsUrl}">
                            Open clinic in Google Maps
                        </a>
                    </p>
                    """;
        
        emailMessage.HtmlBody = html;
        
        await client.EmailSendAsync(emailMessage, stoppingToken);
    }
    
    private static async Task SendAppointmentUpdatedEmailAsync(IResend client, EmailMessage emailMessage, ServiceBusReceivedMessage message, CancellationToken stoppingToken)
    {
        var eventBody = JsonSerializer.Deserialize<AppointmentUpdatedEvent>(message.Body.ToString());
        emailMessage.To.Add(eventBody?.UserEmail ?? throw new InvalidOperationException());
        emailMessage.Subject = $"Appointment for Pet {eventBody.PetName} has been updated";
        
        var mapsUrl =  GoogleMapsApiBaseUrl + eventBody.GooglePlaceId;
        
        var html = $"""
                    <p>
                        Date: {eventBody.AppointmentDate.ToString(DateFormat)}
                    </p>

                    <p>
                        <a href="{mapsUrl}">
                            Open clinic in Google Maps
                        </a>
                    </p>
                    """;
        
        emailMessage.HtmlBody = html;
        
        await client.EmailSendAsync(emailMessage, stoppingToken);
    }

    private static async Task SendAppointmentDeletedEmailAsync(IResend client, EmailMessage emailMessage,
        ServiceBusReceivedMessage message, CancellationToken stoppingToken)
    {
        var eventBody = JsonSerializer.Deserialize<AppointmentDeletedEvent>(message.Body.ToString());
        emailMessage.To.Add(eventBody?.UserEmail ?? throw new InvalidOperationException());
        emailMessage.Subject = $"Appointment for Pet {eventBody.PetName} on has been deleted";
        emailMessage.TextBody = $"Appointment for Pet {eventBody.PetName} on {eventBody.AppointmentDate.ToString(DateFormat)} has been deleted";
        
        await client.EmailSendAsync(emailMessage, stoppingToken);
    }
}