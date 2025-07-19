using MediatR;
using Microsoft.Extensions.Logging;

namespace dotnet_microservices_boilerplate.OrderService.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that logs requests and responses.
/// </summary>
public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {Request}", typeof(TRequest).Name);
        try
        {
            var response = await next();
            _logger.LogInformation("Handled {Request}", typeof(TRequest).Name);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling {Request}", typeof(TRequest).Name);
            throw;
        }
    }
}
