using dotnet_microservices_boilerplate.OrderService.Application.Dtos;
using dotnet_microservices_boilerplate.OrderService.Application.Handlers;
using dotnet_microservices_boilerplate.OrderService.Application.Queries;
using dotnet_microservices_boilerplate.OrderService.Domain.Entities;
using dotnet_microservices_boilerplate.OrderService.Infrastructure.ViewData;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

public class ListOrdersHandlerTests
{
    [Fact]
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
