using PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Handlers;
using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Brokers;
using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Events;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task Handle_Should_Produce_Event_And_Return_Id()
    {
        var broker = Substitute.For<IKafkaBroker>();
        var logger = Substitute.For<ILogger<CreateOrderHandler>>();
        var handler = new CreateOrderHandler(broker, logger);
        var cmd = new CreateOrderCommand(new[] { new CreateOrderItemDto("p", 1, 2m) });

        var id = await handler.Handle(cmd, CancellationToken.None);

        id.Should().NotBeEmpty();
        await broker.Received().ProduceAsync("orders", Arg.Any<OrderCreatedEvent>(), CancellationToken.None);
    }
}
