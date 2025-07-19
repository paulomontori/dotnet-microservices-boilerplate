using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Serilog;
using MediatR;
using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Brokers;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.Data;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.ViewData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands.CreateOrderCommand>());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PWorx.MicroserviceBoilerPlate.OrderService.Application.Behaviors.LoggingBehavior<,>));

var kafkaBootstrap = builder.Configuration.GetValue<string>("Kafka:BootstrapServers") ?? "localhost:9092";
builder.Services.AddSingleton<IKafkaBroker>(_ => new KafkaBroker(kafkaBootstrap));

var connectionString = builder.Configuration.GetConnectionString("Orders") ??
                       "Host=localhost;Database=orders_db;Username=postgres;Password=postgres";
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379");
builder.Services.AddScoped<IOrderViewRepository, OrderViewRepository>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter();
    });

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();
app.MapPrometheusScrapingEndpoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
