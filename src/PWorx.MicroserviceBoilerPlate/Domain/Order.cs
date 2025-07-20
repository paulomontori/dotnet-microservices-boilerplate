namespace PWorx.MicroserviceBoilerPlate.Domain;

/// <summary>
/// Simplified order aggregate used in unit tests and domain demonstrations.
/// In the boilerplate the domain layer owns the business entities and exposes
/// only what the application needs.
/// </summary>
public class Order
{
    /// <summary>
    /// Unique identifier of the order.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Creation timestamp used for demo events.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Items contained in the order. In a real project this would likely be a
    /// collection of value objects but for brevity it is just strings.
    /// </summary>
    public IList<string> Items { get; init; } = new List<string>();
}
