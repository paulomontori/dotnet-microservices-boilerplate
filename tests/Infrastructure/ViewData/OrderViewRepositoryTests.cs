using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Entities;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.Data;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.ViewData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Xunit;
using System;
using System.Threading.Tasks;

public class OrderViewRepositoryTests
{
    private static OrderDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }

    private static IDistributedCache CreateCache() => new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));

    [Fact]
    public async Task GetByIdAsync_Should_Return_From_Cache()
    {
        var context = CreateContext();
        var cache = CreateCache();
        var repo = new OrderViewRepository(context, cache);
        var order = new Order { Id = Guid.NewGuid() };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        var first = await repo.GetByIdAsync(order.Id);
        var second = await repo.GetByIdAsync(order.Id);

        first.Should().NotBeNull();
        second.Should().NotBeNull();
    }

    [Fact]
    public async Task ListAsync_Should_Return_Orders()
    {
        var context = CreateContext();
        var cache = CreateCache();
        var repo = new OrderViewRepository(context, cache);
        context.Orders.Add(new Order { Id = Guid.NewGuid() });
        await context.SaveChangesAsync();

        var result = await repo.ListAsync(null, 1, 10);

        result.Should().HaveCount(1);
    }
}
