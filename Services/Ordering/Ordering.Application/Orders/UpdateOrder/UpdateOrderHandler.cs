using Microsoft.Extensions.Logging;
using Ordering.Application.Abstractions;
using Ordering.Application.Exceptions;
using Ordering.Application.Mapper;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Orders.UpdateOrder
{
    public class UpdateOrderHandler(IOrderRepository orderRepository, ILogger<UpdateOrderHandler> logger) : ICommandHandler<UpdateOrderCommand>
    {
        public async Task Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderToUpdate = await orderRepository.GetByIdAsync(command.Id) ?? throw new OrderNotFoundException(nameof(Order), command.Id);
            orderToUpdate.ApplyUpdate(command);
            await orderRepository.UpdateAsync(orderToUpdate);
        }
    }
}
