using Microsoft.EntityFrameworkCore;
using Polly;

namespace Ordering.API.Extensions
{
    public static class DbExtension
    {
        public static async Task<IHost> MigrateDatabaseAsync<TContext>(this IHost host, Func<TContext, IServiceProvider, Task> seeder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                if (context == null) return host;

                try
                {
                    logger.LogInformation($"Start DB migration {typeof(TContext).Name}");
                    var retry = Policy.Handle<Exception>()
                        .WaitAndRetry(
                            retryCount: 5,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                            onRetry: (exception, timeSpan, retry, ctx) =>
                            {
                                logger.LogWarning($"Retrying because of {exception} {timeSpan}");
                            }
                        );
                    await context!.Database.MigrateAsync();
                    await seeder(context, services);
                    logger.LogInformation($"DB migration {typeof(TContext).Name} completed successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating db: {typeof(TContext).Name}");
                }
            }

            return host;
        }
    }
}
