namespace PWorx.MicroserviceBoilerPlate.Domain;

/// <summary>
/// Contract used by the domain to persist <see cref="Order"/> aggregates.
/// The interface hides any infrastructure concerns so the domain layer
/// does not depend on Entity Framework or any other storage technology.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Persists the given order. Implementations decide whether this means
    /// insert or update. This abstraction allows the domain service to remain
    /// ignorant of the data access strategy.
    /// </summary>
    Task SaveAsync(Order order);

    /// <summary>
    /// Retrieves an order by identifier or returns <c>null</c> when it does not
    /// exist. The repository may query the database or any other persistence
    /// mechanism.
    /// </summary>
    Task<Order?> GetAsync(Guid id);
}
