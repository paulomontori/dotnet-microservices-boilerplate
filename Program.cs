var builder = WebApplication.CreateBuilder(args);

using dotnet_microservices_boilerplate.OrderService.Domain.Brokers;
using dotnet_microservices_boilerplate.OrderService.Infrastructure.Data;
using dotnet_microservices_boilerplate.OrderService.Infrastructure.ViewData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<dotnet_microservices_boilerplate.OrderService.Application.Commands.CreateOrderCommand>());

var kafkaBootstrap = builder.Configuration.GetValue<string>("Kafka:BootstrapServers") ?? "localhost:9092";
builder.Services.AddSingleton<IKafkaBroker>(_ => new KafkaBroker(kafkaBootstrap));

var connectionString = builder.Configuration.GetConnectionString("Orders") ??
                       "Host=localhost;Database=orders_db;Username=postgres;Password=postgres";
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379");
builder.Services.AddScoped<OrderViewRepository>();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
