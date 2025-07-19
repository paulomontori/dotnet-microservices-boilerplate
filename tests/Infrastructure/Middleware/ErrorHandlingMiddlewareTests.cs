using System.Net;
using System.Text.Json;
using PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

public class ErrorHandlingMiddlewareTests
{
    [Test]
    public async System.Threading.Tasks.Task Invoke_Should_Return_NotFound_For_KeyNotFoundException()
    {
        var logger = Substitute.For<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(ctx => throw new KeyNotFoundException("missing"), logger);
        var context = new DefaultHttpContext();
        var stream = new MemoryStream();
        context.Response.Body = stream;

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        stream.Position = 0;
        var body = await JsonDocument.ParseAsync(stream);
        body.RootElement.GetProperty("error").GetString().Should().Be("missing");
    }

    [Test]
    public async Task Invoke_Should_Return_InternalServerError_For_Exception()
    {
        var logger = Substitute.For<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(ctx => throw new InvalidOperationException(), logger);
        var context = new DefaultHttpContext();
        var stream = new MemoryStream();
        context.Response.Body = stream;

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        stream.Position = 0;
        var body = await JsonDocument.ParseAsync(stream);
        body.RootElement.GetProperty("error").GetString().Should().NotBeNull();
    }

    [Test]
    public async Task Invoke_Should_Pass_Through_When_No_Exception()
    {
        var logger = Substitute.For<ILogger<ErrorHandlingMiddleware>>();
        var called = false;
        var middleware = new ErrorHandlingMiddleware(ctx => { called = true; return Task.CompletedTask; }, logger);
        var context = new DefaultHttpContext();

        await middleware.InvokeAsync(context);

        called.Should().BeTrue();
        context.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
