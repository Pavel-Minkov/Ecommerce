using EventBus.Messages.Events;
using MassTransit;

namespace Payment.Consumer
{
    public class OrderCreatedConsumer(IPublishEndpoint publishEndpoint, ILogger<OrderCreatedConsumer> logger) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var message = context.Message;
            logger.LogInformation("Processing payment for order: {OrderId}", message.Id);

            //Simulate payment
            await Task.Delay(2000);
            if (message.TotalPrice > 0)
            {
                //Success
                var completedEvent = new PaymentCompletedEvent
                {   
                    OrderId = message.Id,
                    CorrelationId = message.CorrelationId,
                };
                await publishEndpoint.Publish(completedEvent);
                logger.LogInformation("Payment successful for order: {OrderId} and correlation ID: {CorrelationId}", message.Id, message.CorrelationId);
            }
            else
            {
                var failedEvent = new PaymentFailedEvent
                {
                    OrderId = message.Id,
                    CorrelationId = message.CorrelationId,
                    Reason = "Payment amount must be greater than zero."
                };
                await publishEndpoint.Publish(failedEvent);
                logger.LogInformation("Payment failed for order: {OrderId} and correlation ID: {CorrelationId}", message.Id, message.CorrelationId);
            }
        }
    }
}
