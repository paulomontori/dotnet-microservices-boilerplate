using MediatR;
using dotnet_microservices_boilerplate.OrderService.Application.Dtos;
using dotnet_microservices_boilerplate.OrderService.Application.Queries;
using dotnet_microservices_boilerplate.OrderService.Infrastructure.ViewData;
using Microsoft.Extensions.Logging;

namespace dotnet_microservices_boilerplate.OrderService.Application.Handlers;

public sealed class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly OrderViewRepository _repository;
    private readonly ILogger<GetOrderByIdHandler> _logger;

    public GetOrderByIdHandler(OrderViewRepository repository, ILogger<GetOrderByIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (order is null)
        {
            _logger.LogWarning("Order {OrderId} not found", request.Id);
            throw new KeyNotFoundException($"Order {request.Id} not found");
        }

        var items = order.Items.Select(i => new OrderItemDto(i.Id, i.ProductName ?? string.Empty, i.Quantity, i.UnitPrice)).ToList();
        return new OrderDto(order.Id, order.Status, items);
    }
}
