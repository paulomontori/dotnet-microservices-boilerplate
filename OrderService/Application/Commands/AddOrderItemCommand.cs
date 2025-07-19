using MediatR;

namespace dotnet_microservices_boilerplate.OrderService.Application.Commands;

public sealed record AddOrderItemCommand(Guid OrderId, string ProductName, int Quantity, decimal UnitPrice) : IRequest;
