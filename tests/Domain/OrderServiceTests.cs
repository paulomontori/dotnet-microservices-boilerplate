using System.Threading.Tasks;
using PWorx.MicroserviceBoilerPlate.Domain;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;

public class OrderServiceTests
{
    [Test]
    public async Task CreateOrderAsync_Should_Save_And_Publish()
    {
        var repo = Substitute.For<IOrderRepository>();
        var producer = Substitute.For<IKafkaProducer>();
        var service = new OrderService(repo, producer);
        var order = new Order { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow };

        await service.CreateOrderAsync(order);

        await repo.Received().SaveAsync(order);
        await producer.Received().PublishAsync(Arg.Any<string>());
    }

    [Test]
    public async Task GetOrderAsync_Should_Return_Order_And_Publish()
    {
        var repo = Substitute.For<IOrderRepository>();
        var producer = Substitute.For<IKafkaProducer>();
        var service = new OrderService(repo, producer);
        var order = new Order { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow };
        repo.GetAsync(order.Id).Returns(order);

        var result = await service.GetOrderAsync(order.Id);

        result.Should().Be(order);
        await repo.Received().GetAsync(order.Id);
        await producer.Received().PublishAsync(Arg.Any<string>());
    }
}
