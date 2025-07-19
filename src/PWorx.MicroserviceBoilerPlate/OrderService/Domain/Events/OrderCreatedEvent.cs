namespace PWorx.MicroserviceBoilerPlate.OrderService.Domain.Events;

public sealed record OrderCreatedEvent(Guid OrderId, DateTime Timestamp);
