namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Dtos;

/// <summary>
/// Lightweight representation of an order item returned by the API.
/// </summary>
public sealed record OrderItemDto(Guid Id, string ProductName, int Quantity, decimal UnitPrice);

/// <summary>
/// DTO used by the API layer to expose order data without leaking domain entities.
/// </summary>
public sealed record OrderDto(Guid Id, string Status, IReadOnlyList<OrderItemDto> Items);
