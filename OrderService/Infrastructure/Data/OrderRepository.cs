using Microsoft.EntityFrameworkCore;
using dotnet_microservices_boilerplate.OrderService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace dotnet_microservices_boilerplate.OrderService.Infrastructure.Data;

public sealed class OrderRepository
{
    private readonly OrderDbContext _dbContext;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(OrderDbContext dbContext, ILogger<OrderRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SaveAsync(Order order, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Saving order {OrderId}", order.Id);
        var existing = await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

        if (existing is null)
        {
            await _dbContext.Orders.AddAsync(order, cancellationToken);
        }
        else
        {
            _dbContext.Entry(existing).CurrentValues.SetValues(order);
            existing.Items.Clear();
            foreach (var item in order.Items)
            {
                existing.Items.Add(item);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Order {OrderId} saved", order.Id);
    }

    public async Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Fetching order {OrderId}", id);
        var order = await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (order is null)
        {
            _logger.LogWarning("Order {OrderId} not found", id);
        }
        else
        {
            _logger.LogDebug("Order {OrderId} retrieved", id);
        }
        return order;
    }
}
