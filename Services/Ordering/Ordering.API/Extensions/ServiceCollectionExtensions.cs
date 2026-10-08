using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Abstractions;
using Ordering.Application.Behaviors;
using Ordering.Application.Validators;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;

namespace Ordering.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderingServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString"), sqlOptions => { 
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
                    sqlOptions.MigrationsAssembly(typeof(OrderContext).Assembly.FullName);
                }));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

            services.Scan(scan => scan
                .FromAssemblies(typeof(ICommandHandler<>).Assembly)

                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                );

            services.AddValidatorsFromAssembly(typeof(CreateOrderCommandValidator).Assembly);

            services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
            services.Decorate(typeof(ICommandHandler<,>), typeof(UnhandledExceptionCommandHandlerDecorator<,>));


            return services;
        }
    }
}
