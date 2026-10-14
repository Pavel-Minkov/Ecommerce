using Ordering.Application.Abstractions;
using Ordering.Application.Mapper;
using Ordering.Core.Repositories;

namespace Ordering.Application.Orders.CreateOrder
{
    public class CreateOrderHandler(IOrderRepository orderRepository) : ICommandHandler<CreateOrderCommand, int>
    {
        public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = command.ToEntity();
            var generatedOrder = await orderRepository.AddAsync(order);
            var outboxMessage = OrderMapper.ToOutboxMessage(generatedOrder, command.CorrelationId);
            await orderRepository.AddOutboxMessageAsync(outboxMessage);
            return generatedOrder.Id;
        }
    }
}
