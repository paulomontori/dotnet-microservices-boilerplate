using MediatR;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Dtos;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Queries;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.ViewData;
using Microsoft.Extensions.Logging;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Handlers;

/// <summary>
/// Handles <see cref="GetOrderByIdQuery"/> requests using the read model
/// repository. Queries are separated from commands to keep reads fast and
/// simple.
/// </summary>
public sealed class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IOrderViewRepository _repository;
    private readonly ILogger<GetOrderByIdHandler> _logger;

    /// <summary>
    /// Creates the handler with dependencies provided by DI.
    /// </summary>
    public GetOrderByIdHandler(IOrderViewRepository repository, ILogger<GetOrderByIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves the order from the view repository and maps it to a DTO.
    /// Throws when the order does not exist.
    /// </summary>
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
