namespace dotnet_microservices_boilerplate.Domain;

public interface IOrderRepository
{
    Task SaveAsync(Order order);
    Task<Order?> GetAsync(Guid id);
}
