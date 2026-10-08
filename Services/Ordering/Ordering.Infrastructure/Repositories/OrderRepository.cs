using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository(OrderContext orderContext) : RepositoryBase<Order>(orderContext), IOrderRepository
    {
        public async Task<IReadOnlyList<Order>> GetOrdersByUserNameAsync(string userName)
        {
            return await base.orderContext.Orders.Where(o => o.UserName == userName).AsNoTracking().ToListAsync();
        }
    }
}
