using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.EventBusConsumer
{
    public class PaymentCompletedConsumer(IOrderRepository orderRepository, ILogger<PaymentCompletedConsumer> logger) : IConsumer<PaymentCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var message = context.Message;
            var order = await orderRepository.GetByIdAsync(message.OrderId);
            if (order == null)
            {
                logger.LogWarning("Order not found for Order Id: {@OrderId} and Correlation Id: {@CorrelationId}", message.OrderId, message.CorrelationId);
                return;
            }
            order.Status = OrderStatus.Paid;
            await orderRepository.UpdateAsync(order);
            logger.LogInformation("Order status updated to Paid for Order Id: {@OrderId} and Correlation Id: {@CorrelationId}", message.OrderId, message.CorrelationId);
        }
    }
}
