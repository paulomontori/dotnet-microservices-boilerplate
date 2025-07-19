using MediatR;
using dotnet_microservices_boilerplate.OrderService.Application.Dtos;
using dotnet_microservices_boilerplate.OrderService.Application.Queries;
using dotnet_microservices_boilerplate.OrderService.Infrastructure.ViewData;
using Microsoft.Extensions.Logging;

namespace dotnet_microservices_boilerplate.OrderService.Application.Handlers;

public sealed class ListOrdersHandler : IRequestHandler<ListOrdersQuery, PagedResult<OrderDto>>
{
    private readonly OrderViewRepository _repository;
    private readonly ILogger<ListOrdersHandler> _logger;

    public ListOrdersHandler(OrderViewRepository repository, ILogger<ListOrdersHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PagedResult<OrderDto>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var orders = await _repository.ListAsync(request.Status, page, request.PageSize, cancellationToken);
        _logger.LogDebug("Retrieved {Count} orders", orders.Count);

        var items = orders.Select(o => new OrderDto(
            o.Id,
            o.Status,
            o.Items.Select(i => new OrderItemDto(i.Id, i.ProductName ?? string.Empty, i.Quantity, i.UnitPrice)).ToList()
        )).ToList();

        return new PagedResult<OrderDto>(items, page, request.PageSize, items.Count);
    }
}
