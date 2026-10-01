using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Discount.Infrastructure.Settings
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase(this IHost host) {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("DbMigration");
            var databaseSettings = services.GetRequiredService<IOptions<DatabaseSettings>>().Value;

            try
            {
                logger.LogInformation("Discount database migration started");
                ApplyMigration(databaseSettings.ConnectionString);
                logger.LogInformation("Discount database migration completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while migrating Discount database");
                throw;
            }

            return host;
        }

        private static void ApplyMigration(string connectionString)
        {
            var retry = 5;
            while (retry > 0)
            {
                try
                {
                    using var connection = new Npgsql.NpgsqlConnection(connectionString);
                    connection.Open();
                    using var command = new Npgsql.NpgsqlCommand
                    {
                        Connection = connection
                    };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();
                    
                    command.CommandText = @"
                        CREATE TABLE Coupon (
                            Id SERIAL PRIMARY KEY,
                            ProductName VARCHAR(24) NOT NULL,
                            Description TEXT,
                            Amount INT
                        )";
                    command.ExecuteNonQuery();

                    command.CommandText = @"
                        INSERT INTO Coupon(ProductName, Description, Amount)
                        VALUES('Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500)";
                    command.ExecuteNonQuery();

                    command.CommandText = @"
                        INSERT INTO Coupon(ProductName, Description, Amount)
                        VALUES('Yonex VCORE Pro 100 A Tennis Racquet (270gm, Strung)', 'Racquet Discount', 700)";
                    command.ExecuteNonQuery();

                    break; // Exit the loop if migration is successful
                }
                catch
                {
                    retry--;
                    if (retry == 0)
                    {
                        throw; // Rethrow the exception if all retries are exhausted
                    }
                }
            }
        }
    }
}
