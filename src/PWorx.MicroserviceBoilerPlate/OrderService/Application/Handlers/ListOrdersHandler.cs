using MediatR;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Dtos;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Queries;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.ViewData;
using Microsoft.Extensions.Logging;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Handlers;

/// <summary>
/// Handles <see cref="ListOrdersQuery"/> by retrieving a paged list of orders
/// from the read model repository.
/// </summary>
public sealed class ListOrdersHandler : IRequestHandler<ListOrdersQuery, PagedResult<OrderDto>>
{
    private readonly IOrderViewRepository _repository;
    private readonly ILogger<ListOrdersHandler> _logger;

    /// <summary>
    /// Constructs the handler with its dependencies.
    /// </summary>
    public ListOrdersHandler(IOrderViewRepository repository, ILogger<ListOrdersHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a paginated list of orders optionally filtered by status and
    /// maps them to DTOs.
    /// </summary>
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
