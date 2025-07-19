using PWorx.MicroserviceBoilerPlate.OrderService.Application.Handlers;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Queries;
using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Entities;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.ViewData;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Collections.Generic;

public class GetOrderByIdHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Dto_When_Order_Found()
    {
        var repo = Substitute.For<IOrderViewRepository>();
        var logger = Substitute.For<ILogger<GetOrderByIdHandler>>();
        var handler = new GetOrderByIdHandler(repo, logger);
        var order = new Order { Id = Guid.NewGuid(), Status = "Pending" };
        order.Items.Add(new OrderItem { ProductName = "p", Quantity = 1, UnitPrice = 2m });
        repo.GetByIdAsync(order.Id, CancellationToken.None).Returns(order);

        var dto = await handler.Handle(new GetOrderByIdQuery(order.Id), CancellationToken.None);

        dto.Id.Should().Be(order.Id);
        dto.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Not_Found()
    {
        var repo = Substitute.For<IOrderViewRepository>();
        var logger = Substitute.For<ILogger<GetOrderByIdHandler>>();
        var handler = new GetOrderByIdHandler(repo, logger);
        repo.GetByIdAsync(Arg.Any<Guid>(), CancellationToken.None).Returns((Order?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => handler.Handle(new GetOrderByIdQuery(Guid.NewGuid()), CancellationToken.None));
    }
}
