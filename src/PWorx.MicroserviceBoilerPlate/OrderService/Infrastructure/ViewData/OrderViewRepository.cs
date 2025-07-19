using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Entities;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.Data;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.ViewData;

public interface IOrderViewRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Order>> ListAsync(string? status, int page, int pageSize, CancellationToken cancellationToken = default);
}

public sealed class OrderViewRepository : IOrderViewRepository
{
    private readonly OrderDbContext _dbContext;
    private readonly IDistributedCache _cache;

    public OrderViewRepository(OrderDbContext dbContext, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    private static string CacheKey(Guid id) => $"order:{id}";

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var key = CacheKey(id);
        var cached = await _cache.GetStringAsync(key, cancellationToken);
        if (cached is not null)
        {
            return JsonSerializer.Deserialize<Order>(cached);
        }

        var order = await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (order is not null)
        {
            await _cache.SetStringAsync(
                key,
                JsonSerializer.Serialize(order),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) },
                cancellationToken);
        }

        return order;
    }

    public async Task<IReadOnlyList<Order>> ListAsync(string? status, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var key = $"orders:{status}:{page}:{pageSize}";
        var cached = await _cache.GetStringAsync(key, cancellationToken);
        if (cached is not null)
        {
            var list = JsonSerializer.Deserialize<List<Order>>(cached);
            return list ?? new List<Order>();
        }

        var query = _dbContext.Orders.AsNoTracking().Include(o => o.Items).AsQueryable();
        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(o => o.Status == status);
        }

        var orders = await query
            .OrderBy(o => o.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        await _cache.SetStringAsync(
            key,
            JsonSerializer.Serialize(orders),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) },
            cancellationToken);

        return orders;
    }
}
