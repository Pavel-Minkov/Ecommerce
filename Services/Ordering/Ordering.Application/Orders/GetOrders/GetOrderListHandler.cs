using Ordering.Application.Abstractions;
using Ordering.Application.DTOs;
using Ordering.Application.Mapper;
using Ordering.Core.Repositories;

namespace Ordering.Application.Orders.GetOrders
{
    public class GetOrderListHandler(IOrderRepository orderRepository) : IQueryHandler<GetOrderListQuery, List<OrderDto>>
    {
        public async Task<List<OrderDto>> Handle(GetOrderListQuery query, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetOrdersByUserNameAsync(query.userName);
            return [.. orders.Select(o => o.ToDto())];
        }
    }
}
