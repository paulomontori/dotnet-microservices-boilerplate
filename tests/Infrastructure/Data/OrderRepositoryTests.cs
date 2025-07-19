using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Entities;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using System;
using System.Threading.Tasks;

public class OrderRepositoryTests
{
    private static OrderDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new OrderDbContext(options);
    }

    [Fact]
    public async Task SaveAsync_Should_Add_New_Order()
    {
        var context = CreateContext();
        var logger = Substitute.For<ILogger<OrderRepository>>();
        var repo = new OrderRepository(context, logger);
        var order = new Order { Id = Guid.NewGuid() };

        await repo.SaveAsync(order);

        var saved = await context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
        saved.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAsync_Should_Return_Order()
    {
        var context = CreateContext();
        var logger = Substitute.For<ILogger<OrderRepository>>();
        var repo = new OrderRepository(context, logger);
        var order = new Order { Id = Guid.NewGuid() };
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        var result = await repo.GetAsync(order.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(order.Id);
    }
}
