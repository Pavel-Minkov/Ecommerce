using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seeded the database with initial orders.");
            }
        }
        private static IEnumerable<Order> GetOrders()
        {
            return
            [
                new Order
                {
                    UserName = "Gesha",
                    FirstName = "Gosho",
                    LastName = "Peshev",
                    EmailAddress = "gman@ecommerce.net",
                    AddressLine = "Vitoshka",
                    State = "SF",
                    Country = "Bulgaria",
                    ZipCode = "1000",

                    CardName = "Visa",
                    CardNumber = "4111111111111111",
                    CreatedBy = "Gosho",
                    Expiration = "12/29",
                    Cvv = "123",
                    PaymentMethod = 1,
                    LastModifiedBy = "Gosho",
                    LastModifiedDate = DateTime.UtcNow,
                },
            ];
        }
    }
}
