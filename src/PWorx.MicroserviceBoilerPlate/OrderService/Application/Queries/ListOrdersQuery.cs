using MediatR;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Dtos;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Queries;

public sealed record ListOrdersQuery(string? Status, int Page = 1, int PageSize = 20) : IRequest<PagedResult<OrderDto>>;
