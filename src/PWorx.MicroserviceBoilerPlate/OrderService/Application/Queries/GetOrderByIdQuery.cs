using MediatR;
using PWorx.MicroserviceBoilerPlate.OrderService.Application.Dtos;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Queries;

/// <summary>
/// Query that retrieves an order by its identifier.
/// </summary>
public sealed record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto>;
