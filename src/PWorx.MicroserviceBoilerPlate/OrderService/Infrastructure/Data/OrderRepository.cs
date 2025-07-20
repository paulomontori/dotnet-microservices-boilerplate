using Microsoft.EntityFrameworkCore;
using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.Data;

/// <summary>
/// Concrete repository implementation using Entity Framework Core. The domain
/// only sees the <c>OrderRepository</c> through the <see cref="OrderDbContext"/>
/// abstraction.
/// </summary>
public sealed class OrderRepository
{
    private readonly OrderDbContext _dbContext;
    private readonly ILogger<OrderRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the repository with its dependencies.
    /// </summary>
    public OrderRepository(OrderDbContext dbContext, ILogger<OrderRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Adds or updates an order and saves changes to the database.
    /// </summary>
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

    /// <summary>
    /// Retrieves an order with its items by identifier.
    /// </summary>
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
