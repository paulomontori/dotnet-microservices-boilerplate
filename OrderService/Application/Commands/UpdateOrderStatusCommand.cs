using MediatR;

namespace dotnet_microservices_boilerplate.OrderService.Application.Commands;

public sealed record UpdateOrderStatusCommand(Guid OrderId, string Status) : IRequest;
