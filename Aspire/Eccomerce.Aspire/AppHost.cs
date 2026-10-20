using Projects;

var builder = DistributedApplication.CreateBuilder(args);

//Infrastructure
//Catalog
var mongo = builder.AddMongoDB("catalogdb").WithDataVolume();
//Basket
var redis = builder.AddRedis("basketdb");
//Discount
var postgres = builder.AddPostgres("discount-postgres").WithDataVolume();
var discountDb = postgres.AddDatabase("DiscountDb");
//Ordering + Identity
var sqlServer = builder.AddSqlServer("sqlserver").WithDataVolume();
var orderingDb = sqlServer.AddDatabase("OrderingDb");
var identityDb = sqlServer.AddDatabase("IdentityDb");
//Basket, Ordering, Payment
var rabbitMq = builder.AddRabbitMQ("rabbitmq");

//APIs
var catalog = builder.AddProject<Catalog_API>("catalog").WithReference(mongo);
var basket = builder.AddProject<Basket_API>("basket").WithReference(redis).WithReference(rabbitMq);
var ordering = builder.AddProject<Ordering_API>("ordering").WithReference(orderingDb).WithReference(rabbitMq);
var identity = builder.AddProject<Identity>("identity")
    .WithReference(identityDb)
    .WithEnvironment("Jwt__Key", "super_secure_secret_key_1234567890!@#")
    .WithEnvironment("Jwt__Issuer", "ecommerce.identity")
    .WithEnvironment("Jwt__Audience", "ecommerce.clients")
    .WithEnvironment("Jwt__DurationInMinutes", "60"); ;
var payment = builder.AddProject<Payment>("payment").WithReference(rabbitMq);
var discount = builder.AddProject<Discount_API>("discount").WithReference(discountDb);

//Add Ocelot API Gateway
var gateway = builder.AddProject<ApiGateway>("apigateway");

//Express the dependencies between the projects
gateway
    .WithReference(catalog)
    .WithReference(basket)
    .WithReference(ordering)
    .WithReference(identity)
    .WithReference(payment)
    .WithReference(discount);

builder.Build().Run();
