using EventBus.Messages.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Extensions;
using Ordering.Application.Dispatcher;
using Ordering.Application.EventBusConsumer;
using Ordering.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Ordering services
builder.Services.AddOrderingServices(builder.Configuration);

//Add OutboxMessageDispatcher as a hosted service
builder.Services.AddHostedService<OutboxMessageDispatcher>();

//Add MassTransit
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketOrderingConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetSection("EventBusSettings").GetValue<string>("HostAddress"));
        cfg.ReceiveEndpoint(EventBusConstant.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketOrderingConsumer>(ctx);
        });
    });
});

var app = builder.Build();
if (!EF.IsDesignTime)
{
    await app.MigrateDatabaseAsync<OrderContext>(async (context, services) =>
    {
        var logger = services.GetService<ILogger<OrderContextSeed>>();
        await OrderContextSeed.SeedAsync(context, logger);
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
