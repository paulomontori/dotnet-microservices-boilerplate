using dotnet_microservices_boilerplate.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton(_ => new KafkaProducer("localhost:9092", "orders"));
builder.Services.AddSingleton<OrderService>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
