using MediatR;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;

public sealed record CancelOrderCommand(Guid OrderId) : IRequest;
