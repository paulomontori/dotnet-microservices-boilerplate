namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Dtos;

/// <summary>
/// Helper DTO used for returning paginated data from query handlers.
/// </summary>
public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount);
