using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Repositiries;
using Discount.Infrastructure.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var assemblies = new Assembly[] { Assembly.GetExecutingAssembly(), typeof(CreateDiscountHandler).Assembly };
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddGrpc();
var app = builder.Build();

app.MigrateDatabase();
app.UseRouting();
app.MapGrpcService<DiscountService>();

app.Run();
