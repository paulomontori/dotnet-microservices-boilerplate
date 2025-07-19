using MediatR;
using dotnet_microservices_boilerplate.OrderService.Application.Dtos;

namespace dotnet_microservices_boilerplate.OrderService.Application.Queries;

public sealed record ListOrdersQuery(string? Status, int Page = 1, int PageSize = 20) : IRequest<PagedResult<OrderDto>>;
