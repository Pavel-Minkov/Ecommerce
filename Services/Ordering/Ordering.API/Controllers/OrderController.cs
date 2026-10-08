using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.CreateOrder;
using Ordering.Application.Orders.DeleteOrder;
using Ordering.Application.Orders.GetOrders;
using Ordering.Application.Orders.UpdateOrder;
using Ordering.Application.DTOs;
using Ordering.Application.Mapper;
using Ordering.Application.Abstractions;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public partial class OrderController(
      ICommandHandler<CreateOrderCommand, int> createOrderHandler,
      ICommandHandler<UpdateOrderCommand> updateOrderHandler,
      ICommandHandler<DeleteOrderCommand> deleteOrderHandler,
      IQueryHandler<GetOrderListQuery, List<OrderDto>> getOrderListHandler,
      ILogger<OrderController> logger) : ControllerBase
    {

        [HttpGet("{userName}", Name = "GetOrdersByUserName")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByUserName(string userName, CancellationToken cancellationToken)
        {
            var query = new GetOrderListQuery(userName);

            var orders = await getOrderListHandler.Handle(query, cancellationToken);

            LogOrderFetched(userName);
            return Ok(orders);
        }

        // Testing purpose
        [HttpPost(Name = "CheckoutOrder")]
        public async Task<ActionResult<int>> CheckoutOrder(
            [FromBody] CreateOrderDto dto,
            CancellationToken cancellationToken)
        {
            var command = dto.ToCommand();

            var orderId = await createOrderHandler.Handle(command, cancellationToken);

            LogOrderCreated(orderId);
            return Ok(orderId);
        }

        [HttpPut(Name = "UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(
            [FromBody] OrderDto dto,
            CancellationToken cancellationToken)
        {
            var command = dto.ToCommand();

            await updateOrderHandler.Handle(command, cancellationToken);

            LogOrderUpdated(dto.Id);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(
            int id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteOrderCommand(id);

            await deleteOrderHandler.Handle(command, cancellationToken);

            LogOrderDeleted(id);

            return NoContent();
        }
        // The source generator writes the high-performance implementation for this in the background!
        [LoggerMessage(Level = LogLevel.Information, Message = "Orders fetched for user {@UserName}")]
        private partial void LogOrderFetched(string userName);
        [LoggerMessage(Level = LogLevel.Information, Message = "Order created with Id {OrderId}")]
        private partial void LogOrderCreated(int orderId);

        [LoggerMessage(Level = LogLevel.Information, Message = "Order updated with Id {OrderId}")]
        private partial void LogOrderUpdated(int orderId);

        [LoggerMessage(Level = LogLevel.Information, Message = "Order deleted with Id {OrderId}")]
        private partial void LogOrderDeleted(int orderId);

    }
}