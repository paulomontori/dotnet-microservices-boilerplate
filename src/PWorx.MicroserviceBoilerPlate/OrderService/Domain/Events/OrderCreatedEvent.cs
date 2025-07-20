namespace PWorx.MicroserviceBoilerPlate.OrderService.Domain.Events;

/// <summary>
/// Event emitted when a new order is created. The event is immutable and can
/// be serialized for publishing to external systems.
/// </summary>
public sealed record OrderCreatedEvent(Guid OrderId, DateTime Timestamp);
