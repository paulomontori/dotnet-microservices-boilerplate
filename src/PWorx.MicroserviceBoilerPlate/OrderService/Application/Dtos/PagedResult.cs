namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Dtos;

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount);
