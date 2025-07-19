using MediatR;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;
using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Brokers;
using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Entities;
using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Events;
using Microsoft.Extensions.Logging;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Handlers;

public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IKafkaBroker _kafkaBroker;
    private readonly ILogger<CreateOrderHandler> _logger;

    public CreateOrderHandler(IKafkaBroker kafkaBroker, ILogger<CreateOrderHandler> logger)
    {
        _kafkaBroker = kafkaBroker;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // For demo purposes simply generate a new order id
        var order = new Order { Id = Guid.NewGuid() };
        foreach (var item in request.Items)
        {
            order.Items.Add(new OrderItem
            {
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });
        }
        // In a real application the order would be persisted using a repository

        // Publish event to Kafka
        var evt = new OrderCreatedEvent(order.Id, DateTime.UtcNow);
        await _kafkaBroker.ProduceAsync("orders", evt, cancellationToken);
        _logger.LogInformation("Order {OrderId} created", order.Id);

        return order.Id;
    }
}
