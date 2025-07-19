using Microsoft.EntityFrameworkCore;
using dotnet_microservices_boilerplate.OrderService.Domain.Entities;

namespace dotnet_microservices_boilerplate.OrderService.Infrastructure.Data;

public sealed class OrderRepository
{
    private readonly OrderDbContext _dbContext;

    public OrderRepository(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(Order order, CancellationToken cancellationToken = default)
    {
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
    }

    public async Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}
