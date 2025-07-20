using MediatR;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;

/// <summary>
/// Command used to append a new item to an existing order. Commands represent
/// intent to change state and are handled by MediatR handlers in the
/// application layer.
/// </summary>
public sealed record AddOrderItemCommand(Guid OrderId, string ProductName, int Quantity, decimal UnitPrice) : IRequest;
