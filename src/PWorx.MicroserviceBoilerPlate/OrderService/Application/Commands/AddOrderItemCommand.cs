using MediatR;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;

public sealed record AddOrderItemCommand(Guid OrderId, string ProductName, int Quantity, decimal UnitPrice) : IRequest;
