using Ordering.Application.Abstractions;
using Ordering.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Orders.GetOrders
{
    public record GetOrderListQuery(string userName) : IQuery<List<OrderDto>>
    {
    }
}
