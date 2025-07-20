using MediatR;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;

/// <summary>
/// Command representing the intent to cancel an existing order.
/// </summary>
public sealed record CancelOrderCommand(Guid OrderId) : IRequest;
