using Projects;

var builder = DistributedApplication.CreateBuilder(args);


// MongoDB ? Catalog
var mongo = builder.AddMongoDB("catalogdb")
                   .WithDataVolume();

// Redis ? Basket
var redis = builder.AddRedis("basketdb");

// PostgreSQL ? Discount
var postgres = builder.AddPostgres("discount-postgres")
                      .WithDataVolume();
var discountDb = postgres.AddDatabase("discountdb");

// SQL Server ? Ordering + Identity
var sqlServer = builder.AddSqlServer("sqlserver")
                       .WithDataVolume();
var orderDb = sqlServer.AddDatabase("orderdb");
var identityDb = sqlServer.AddDatabase("identitydb");

// RabbitMQ
var rabbitMq = builder.AddRabbitMQ("rabbitmq");





// APIs

var catalog = builder.AddProject<Catalog_API>("catalog").WithReference(mongo);
var discount = builder.AddProject<Discount_API>("discount").WithReference(postgres);
var basket = builder.AddProject<Basket_API>("basket").WithReference(redis).WithReference(discount).WithReference(rabbitMq);

// Ordering
var ordering = builder.AddProject<Ordering_API>("ordering")
    .WithReference(orderDb)
    .WithReference(rabbitMq);

// Payment
var payment = builder.AddProject<Payment>("payment")
    .WithReference(rabbitMq);


// API Gateways

var gateway = builder.AddProject<ApiGateway>("gateway");


// optionals: express dependencies explicitly

gateway.WithReference(catalog).WithReference(ordering).WithReference(basket).WithReference(discount).WithReference(payment);
builder.Build().Run();
