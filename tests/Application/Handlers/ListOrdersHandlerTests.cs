using PWorx.MicroserviceBoilerPlate.OrderService.Application.Dtos;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Handlers;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Queries;
using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Entities;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.ViewData;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;

public class ListOrdersHandlerTests
{
    [Test]
    public async Task Handle_Should_Return_PagedResult()
    {
        var repo = Substitute.For<IOrderViewRepository>();
        var logger = Substitute.For<ILogger<ListOrdersHandler>>();
        var handler = new ListOrdersHandler(repo, logger);
        var orders = new List<Order> { new Order { Id = Guid.NewGuid() } };
        repo.ListAsync(null, 1, 20, CancellationToken.None).Returns(orders);

        var result = await handler.Handle(new ListOrdersQuery(null), CancellationToken.None);

        result.Items.Should().HaveCount(1);
        result.Page.Should().Be(1);
        result.TotalCount.Should().Be(1);
    }
}
