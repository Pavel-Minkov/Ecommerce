using Microsoft.Extensions.Logging;
using Ordering.Application.Abstractions;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System.Windows.Input;

namespace Ordering.Application.Orders.DeleteOrder
{
    public class DeleteOrderHandler(IOrderRepository orderRepository, ILogger<DeleteOrderHandler> logger) : ICommandHandler<DeleteOrderCommand>
    {
        public async Task Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var entity = await orderRepository.GetByIdAsync(command.Id) ?? throw new OrderNotFoundException(nameof(Order), command.Id);
            await orderRepository.DeleteAsync(entity);
            logger.LogInformation("Order {OrderId} is successfully deleted.", entity.Id);
        }
    }
}
