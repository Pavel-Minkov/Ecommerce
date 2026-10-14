using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Data;
using System.Text.Json;

namespace Ordering.Application.Dispatcher
{
    public class OutboxMessageDispatcher(IServiceProvider serviceProvider, ILogger<OutboxMessageDispatcher> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
                var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                var pendingMessages = await dbContext.OutboxMessages
                                                                    .Where(m => m.ProcessedOn == null && m.RetryCount < 3)
                                                                    .OrderBy(m => m.OccurredOn)
                                                                    .Take(20)
                                                                    .ToListAsync(cancellationToken: stoppingToken);
                foreach (var message in pendingMessages) {
                    OrderCreatedEvent? orderCreatedEvent = null;
                    bool isPublished = false;
                    try
                    {
                        orderCreatedEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(message.Content);
                        if (orderCreatedEvent != null)
                        {
                            await publishEndpoint.Publish(orderCreatedEvent, stoppingToken);
                            isPublished = true;
                        }
                        else
                        {
                            logger.LogWarning("Message content was empty or failed to deserialize.");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error processing outbox message with ID {MessageId}", message.Id);
                        message.RetryCount++;
                        message.Error = ex.Message;
                    }
                    if (isPublished)
                    {
                        message.ProcessedOn = DateTime.UtcNow;
                        logger.LogInformation("Successfully processed outbox message with ID {MessageId}", message.Id);
                    }
                }
                await dbContext.SaveChangesAsync(stoppingToken);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
