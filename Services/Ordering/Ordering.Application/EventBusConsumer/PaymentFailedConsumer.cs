using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
namespace Ordering.Application.EventBusConsumer
{
    public class PaymentFailedConsumer(IOrderRepository orderRepository, ILogger<PaymentFailedConsumer> logger) : IConsumer<PaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var message = context.Message;
            var order = await orderRepository.GetByIdAsync(message.OrderId);
            if (order == null)
            {
                logger.LogWarning("Order not found for Order Id: {@OrderId} and Correlation Id: {@CorrelationId}", message.OrderId, message.CorrelationId);
                return;
            }
            order.Status = OrderStatus.Failed;
            await orderRepository.UpdateAsync(order);
            logger.LogWarning("Order status updated to Failed for Order Id: {@OrderId} and Correlation Id: {@CorrelationId}", message.OrderId, message.CorrelationId);
        }
    }
}
