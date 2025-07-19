using MediatR;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Application.Commands;

public sealed record RemoveOrderItemCommand(Guid OrderId, Guid ItemId) : IRequest;
