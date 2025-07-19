namespace dotnet_microservices_boilerplate.OrderService.Application.Dtos;

public sealed record OrderItemDto(Guid Id, string ProductName, int Quantity, decimal UnitPrice);

public sealed record OrderDto(Guid Id, string Status, IReadOnlyList<OrderItemDto> Items);
