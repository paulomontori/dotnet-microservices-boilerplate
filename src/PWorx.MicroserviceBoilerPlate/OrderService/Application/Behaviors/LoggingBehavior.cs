using MediatR;
using Microsoft.Extensions.Logging;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that logs requests and responses. Behaviors allow
/// cross-cutting concerns to be applied to all commands and queries without
/// cluttering handlers. Logging here keeps the handlers focused on business
/// logic.
/// </summary>
public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Constructs the behavior with the logger injected by DI.
    /// </summary>
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Executes the next handler in the pipeline while logging the request and
    /// handling any exceptions. This keeps diagnostics consistent for all
    /// operations.
    /// </summary>
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
