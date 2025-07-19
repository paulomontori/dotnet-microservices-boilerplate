namespace PWorx.MicroserviceBoilerPlate.Domain;

public interface IOrderRepository
{
    Task SaveAsync(Order order);
    Task<Order?> GetAsync(Guid id);
}
