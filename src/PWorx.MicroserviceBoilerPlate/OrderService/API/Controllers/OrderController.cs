using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Queries;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Dtos;

namespace PWorx.MicroserviceBoilerPlate.OrderService.API.Controllers;

/// <summary>
/// HTTP API controller exposing order commands and queries. It delegates work
/// to MediatR so that the controller remains thin and free of business logic.
/// </summary>
[ApiController]
[Route("api/v1/orders")]
[Authorize]
public sealed class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor receives <see cref="IMediator"/> so that this controller
    /// simply forwards commands and queries to the application layer.
    /// </summary>
    public OrderController(IMediator mediator) => _mediator = mediator;

    // ----------  QUERIES  ----------
    /// <summary>
    /// Get order by identifier.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    /// <returns>The order details.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid id) =>
        Ok(await _mediator.Send(new GetOrderByIdQuery(id)));

    /// <summary>
    /// List orders with optional paging and filtering.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<OrderDto>>> List([FromQuery] ListOrdersQuery query) =>
        Ok(await _mediator.Send(query));

    // ----------  COMMANDS  ----------
    /// <summary>
    /// Create a new order.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateOrderCommand cmd)
    {
        var orderId = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id = orderId }, orderId);
    }

    /// <summary>
    /// Add an item to the order.
    /// </summary>
    [HttpPost("{id:guid}/items")]
    public async Task<IActionResult> AddItem(Guid id, [FromBody] AddOrderItemCommand cmd)
    {
        await _mediator.Send(cmd with { OrderId = id });
        return NoContent();
    }

    /// <summary>
    /// Remove an item from the order.
    /// </summary>
    [HttpDelete("{id:guid}/items/{itemId:guid}")]
    public async Task<IActionResult> RemoveItem(Guid id, Guid itemId)
    {
        await _mediator.Send(new RemoveOrderItemCommand(id, itemId));
        return NoContent();
    }

    /// <summary>
    /// Update the status of an order.
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusCommand cmd)
    {
        await _mediator.Send(cmd with { OrderId = id });
        return NoContent();
    }

    /// <summary>
    /// Cancel an order.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await _mediator.Send(new CancelOrderCommand(id));
        return NoContent();
    }
}
