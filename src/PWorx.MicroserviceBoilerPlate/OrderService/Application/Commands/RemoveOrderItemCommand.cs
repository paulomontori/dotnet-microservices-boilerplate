using MediatR;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;

/// <summary>
/// Command to remove an item from an order.
/// </summary>
public sealed record RemoveOrderItemCommand(Guid OrderId, Guid ItemId) : IRequest;
