using MediatR;

namespace dotnet_microservices_boilerplate.OrderService.Application.Commands;

public sealed record CancelOrderCommand(Guid OrderId) : IRequest;
