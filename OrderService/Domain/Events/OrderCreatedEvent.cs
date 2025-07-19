namespace dotnet_microservices_boilerplate.OrderService.Domain.Events;

public sealed record OrderCreatedEvent(Guid OrderId, DateTime Timestamp);
