using dotnet_microservices_boilerplate.OrderService.Domain.Entities;

namespace dotnet_microservices_boilerplate.OrderService.Infrastructure.Data;

public sealed class OrderRepository
{
    private static readonly Dictionary<Guid, Order> _store = new();

    public Task SaveAsync(Order order, CancellationToken cancellationToken = default)
    {
        _store[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _store.TryGetValue(id, out var order);
        return Task.FromResult(order);
    }
}
