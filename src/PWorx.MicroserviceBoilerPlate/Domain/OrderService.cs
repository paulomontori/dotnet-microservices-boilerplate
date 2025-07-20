using System.Text.Json;

namespace PWorx.MicroserviceBoilerPlate.Domain;

/// <summary>
/// Domain service that orchestrates order creation and retrieval. By keeping
/// the service in the domain layer we encapsulate business rules while still
/// allowing the application layer to invoke domain logic via MediatR handlers.
/// </summary>
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IKafkaProducer _producer;

    /// <summary>
    /// Creates a new <see cref="OrderService"/> with the required dependencies.
    /// Dependencies are injected via the constructor to adhere to the
    /// Dependency Inversion Principle.
    /// </summary>
    public OrderService(IOrderRepository repository, IKafkaProducer producer)
    {
        _repository = repository;
        _producer = producer;
    }

    /// <summary>
    /// Persists a new order and publishes an integration event. Publishing the
    /// message here allows the infrastructure layer to remain unaware of domain
    /// semantics.
    /// </summary>
    public async Task CreateOrderAsync(Order order)
    {
        await _repository.SaveAsync(order);
        var message = JsonSerializer.Serialize(new { Event = "OrderCreated", OrderId = order.Id, Timestamp = DateTime.UtcNow });
        await _producer.PublishAsync(message);
    }

    /// <summary>
    /// Retrieves an order and emits an event indicating it was fetched.
    /// </summary>
    public async Task<Order?> GetOrderAsync(Guid id)
    {
        var order = await _repository.GetAsync(id);
        var message = JsonSerializer.Serialize(new { Event = "OrderFetched", OrderId = id, Timestamp = DateTime.UtcNow });
        await _producer.PublishAsync(message);
        return order;
    }
}
