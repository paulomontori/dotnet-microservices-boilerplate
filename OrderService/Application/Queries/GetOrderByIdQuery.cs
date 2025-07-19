using MediatR;
using dotnet_microservices_boilerplate.OrderService.Application.Dtos;

namespace dotnet_microservices_boilerplate.OrderService.Application.Queries;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto>;
