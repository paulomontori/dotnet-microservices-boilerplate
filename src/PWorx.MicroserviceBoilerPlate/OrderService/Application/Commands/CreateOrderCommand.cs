using MediatR;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;

public sealed record CreateOrderCommand(IEnumerable<CreateOrderItemDto> Items) : IRequest<Guid>;

public sealed record CreateOrderItemDto(string ProductName, int Quantity, decimal UnitPrice);
