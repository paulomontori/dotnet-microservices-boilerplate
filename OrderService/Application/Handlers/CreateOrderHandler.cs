using MediatR;
using dotnet_microservices_boilerplate.OrderService.Application.Commands;
using dotnet_microservices_boilerplate.OrderService.Domain.Entities;

namespace dotnet_microservices_boilerplate.OrderService.Application.Handlers;

public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    public Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
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
        return Task.FromResult(order.Id);
    }
}
