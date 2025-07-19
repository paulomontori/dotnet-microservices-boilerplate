using System.Threading;
using System.Threading.Tasks;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Behaviors;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

public class LoggingBehaviorTests
{
    [Fact]
    public async Task Handle_Should_Log_And_Return_Response()
    {
        var logger = Substitute.For<ILogger<LoggingBehavior<string, int>>>();
        var behavior = new LoggingBehavior<string, int>(logger);
        var next = Substitute.For<RequestHandlerDelegate<int>>();
        next.Invoke().Returns(42);

        var result = await behavior.Handle("req", next, CancellationToken.None);

        result.Should().Be(42);
        logger.Received().Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object?>(),
            null,
            Arg.Any<Func<object, Exception?, string>>());
        logger.Received().Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object?>(),
            null,
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task Handle_Should_Log_Error_On_Exception()
    {
        var logger = Substitute.For<ILogger<LoggingBehavior<string, int>>>();
        var behavior = new LoggingBehavior<string, int>(logger);
        var next = Substitute.For<RequestHandlerDelegate<int>>();
        next.Invoke().Returns<int>(x => { throw new InvalidOperationException(); });

        await Assert.ThrowsAsync<InvalidOperationException>(() => behavior.Handle("req", next, CancellationToken.None));

        logger.Received().Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Any<object?>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
