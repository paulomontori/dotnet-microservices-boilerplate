using MediatR;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;

public sealed record UpdateOrderStatusCommand(Guid OrderId, string Status) : IRequest;
