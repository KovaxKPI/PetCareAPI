using PetCareAPI.Application.Repositories;
using PetCareAPI.Application.Services;

namespace PetCareAPI.BackgroundServices;

public class OutboxWorker(IServiceScopeFactory scopeFactory, IMessagePublisher publisher, ILogger<OutboxWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var repository =
                    scope.ServiceProvider
                        .GetRequiredService<IOutboxRepository>();

                var messages =
                    await repository.GetPendingMessagesAsync(stoppingToken);

                foreach (var message in messages)
                {
                    await publisher.PublishAppointmentEventAsync(
                        message,
                        stoppingToken);

                    await repository.MarkMessageProcessedAsync(
                        message.MessageId,
                        stoppingToken);
                    
                    logger.LogInformation($"Processed message {message.MessageId}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Outbox processing failed");
            }

            await Task.Delay(
                TimeSpan.FromSeconds(10),
                stoppingToken);
        }
    }
}