using MediatR;

namespace dotnet_microservices_boilerplate.OrderService.Application.Commands;

public sealed record RemoveOrderItemCommand(Guid OrderId, Guid ItemId) : IRequest;
