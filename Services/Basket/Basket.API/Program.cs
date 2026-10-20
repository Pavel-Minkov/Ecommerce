using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Application.Settings;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Basket.Infrastructure.Settings;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddServiceDefaults();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Mediatr
var assemblies = new Assembly[] { 
    Assembly.GetExecutingAssembly(),
    typeof(CreateShoppingCartHandler).Assembly
};
builder.Services.AddMediatR(cfg =>cfg.RegisterServicesFromAssemblies(assemblies));

//Options pattern
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
builder.Services.Configure<GrpcSettings>(builder.Configuration.GetSection("GrpcSettings"));

//Register Grpc client for Discount service
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>((sp,cfg) =>
{
    var grpcSettings = sp.GetRequiredService<IOptions<GrpcSettings>>().Value;
    cfg.Address = new Uri(grpcSettings.DiscountUrl);
});

builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddStackExchangeRedisCache((options) =>
{
    options.Configuration = builder.Configuration.GetSection("CacheSettings").GetValue<string>("ConnectionString");
});

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetSection("EventBusSettings").GetValue<string>("HostAddress"));
    });
});

builder.Host.UseSerilog(Logging.ConfigureLogging);

var app = builder.Build();

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
