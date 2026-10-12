using Azure.Messaging.ServiceBus;
using PetCareAPI.EmailService.Services;

namespace PetCareAPI.EmailService.BackgroundServices;

public class OutboxConsumer(ServiceBusClient client, IEmailService emailService, ILogger<OutboxConsumer> logger) : BackgroundService
{
    private const string QueueName = "appointment-queue";
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ConsumeFromQueueAsync(stoppingToken);
    }

    private async Task ConsumeFromQueueAsync(CancellationToken stoppingToken)
    {
        await using var processor = client.CreateProcessor(QueueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 5
        });
        
        processor.ProcessMessageAsync += ProcessMessageAsync;
        processor.ProcessErrorAsync += ProcessErrorAsync;
        
        await processor.StartProcessingAsync(stoppingToken);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
        }
        
        await processor.StopProcessingAsync(stoppingToken);
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        try
        {
            var messageBody = args.Message.Body.ToString();
            logger.LogInformation("Outbox Message: {MessageBody}", messageBody);
                
            await emailService.SendEmailAsync(args.Message, args.CancellationToken);
                
            await Task.Delay(100, args.CancellationToken);

            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in Outbox");
            await args.AbandonMessageAsync(args.Message);
        }
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        logger.LogError(args.Exception, "Error in Outbox");
        return Task.CompletedTask;
    }
}