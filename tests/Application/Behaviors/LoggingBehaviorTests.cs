using System.Threading;
using System.Threading.Tasks;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Behaviors;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;

public class LoggingBehaviorTests
{
    [Test]
    public async Task Handle_Should_Log_And_Return_Response()
    {
        var logger = Substitute.For<ILogger<LoggingBehavior<string, int>>>();
        var behavior = new LoggingBehavior<string, int>(logger);
        var next = Substitute.For<RequestHandlerDelegate<int>>();
        next.Invoke().Returns(42);

        var result = await behavior.Handle("req", next, CancellationToken.None);

        result.Should().Be(42);
    }

    [Test]
    public void Handle_Should_Log_Error_On_Exception()
    {
        var logger = Substitute.For<ILogger<LoggingBehavior<string, int>>>();
        var behavior = new LoggingBehavior<string, int>(logger);
        var next = Substitute.For<RequestHandlerDelegate<int>>();
        next.Invoke().Returns<int>(x => { throw new InvalidOperationException(); });

        Assert.ThrowsAsync<InvalidOperationException>(async () => await behavior.Handle("req", next, CancellationToken.None));
    }
}
