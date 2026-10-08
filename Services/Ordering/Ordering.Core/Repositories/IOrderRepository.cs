using Ordering.Core.Entities;

namespace Ordering.Core.Repositories
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<IReadOnlyList<Order>> GetOrdersByUserNameAsync(string userName);
    }
}
