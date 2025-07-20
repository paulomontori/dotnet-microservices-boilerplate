using MediatR;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;

/// <summary>
/// Command used to create a brand new order along with its initial items.
/// </summary>
public sealed record CreateOrderCommand(IEnumerable<CreateOrderItemDto> Items) : IRequest<Guid>;

/// <summary>
/// DTO that represents an item in the <see cref="CreateOrderCommand"/>.
/// </summary>
public sealed record CreateOrderItemDto(string ProductName, int Quantity, decimal UnitPrice);
