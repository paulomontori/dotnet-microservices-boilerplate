using Microsoft.AspNetCore.Mvc;
using dotnet_microservices_boilerplate.Domain;

namespace dotnet_microservices_boilerplate.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;

    public OrdersController(OrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        if (order.Id == Guid.Empty)
        {
            order = order with { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow };
        }
        await _service.CreateOrderAsync(order);
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var order = await _service.GetOrderAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }
}
