using MediatR;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;

/// <summary>
/// Command that updates the status of an order (e.g. shipped, cancelled).
/// </summary>
public sealed record UpdateOrderStatusCommand(Guid OrderId, string Status) : IRequest;
