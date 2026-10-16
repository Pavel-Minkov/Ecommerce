using Common.Logging;
using EventBus.Messages.Common;
using MassTransit;
using Payment.Consumer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<OrderCreatedConsumer>();
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint(EventBusConstant.OrderCreatedQueue, e =>
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
    });
});

builder.Host.UseSerilog(Logging.ConfigureLogging);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
