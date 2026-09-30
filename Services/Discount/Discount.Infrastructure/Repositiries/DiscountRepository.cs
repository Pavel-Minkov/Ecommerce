using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Discount.Infrastructure.Repositiries
{
    public class DiscountRepository(IOptions<DatabaseSettings> databaseSettings) : IDiscountRepository
    {
        private readonly string _connection_string = databaseSettings.Value.ConnectionString;

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_connection_string);
            var affected = await connection.ExecuteAsync(
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { coupon.ProductName, coupon.Description, coupon.Amount });
            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_connection_string);
            var affected = await connection.ExecuteAsync(
                "DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });
            return affected > 0;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_connection_string);
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });
            return coupon ?? new Coupon 
            {
                ProductName = "No Discount",
                Description = "No Discount Found",
                Amount = 0
            };
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            await using var connection = new Npgsql.NpgsqlConnection(_connection_string);
            var affected = await connection.ExecuteAsync(
                "UPDATE Coupon SET Description = @Description, Amount = @Amount WHERE ProductName = @ProductName",
                new { coupon.Description, coupon.Amount, coupon.ProductName });
            return affected > 0;
        }
    }
}
