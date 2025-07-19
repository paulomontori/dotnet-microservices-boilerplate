using MediatR;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Dtos;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Queries;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto>;
