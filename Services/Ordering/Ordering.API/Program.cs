using Ordering.API.Extensions;
using Ordering.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Ordering services
builder.Services.AddOrderingServices(builder.Configuration);

var app = builder.Build();

await app.MigrateDatabaseAsync<OrderContext>(async (context, services) =>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    await OrderContextSeed.SeedAsync(context, logger);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
