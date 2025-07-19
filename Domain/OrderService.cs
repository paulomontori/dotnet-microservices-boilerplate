using System.Text.Json;

namespace dotnet_microservices_boilerplate.Domain;

public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly KafkaProducer _producer;

    public OrderService(IOrderRepository repository, KafkaProducer producer)
    {
        _repository = repository;
        _producer = producer;
    }

    public async Task CreateOrderAsync(Order order)
    {
        await _repository.SaveAsync(order);
        var message = JsonSerializer.Serialize(new { Event = "OrderCreated", OrderId = order.Id, Timestamp = DateTime.UtcNow });
        await _producer.PublishAsync(message);
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        var order = await _repository.GetAsync(id);
        var message = JsonSerializer.Serialize(new { Event = "OrderFetched", OrderId = id, Timestamp = DateTime.UtcNow });
        await _producer.PublishAsync(message);
        return order;
    }
}
