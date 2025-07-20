namespace PWorx.MicroserviceBoilerPlate.OrderService.Domain.Entities;

/// <summary>
/// Aggregate root representing an order in the OrderService bounded context.
/// This entity is intentionally small to keep the example focused on
/// infrastructure and messaging concerns.
/// </summary>
public sealed class Order
{
    /// <summary>
    /// Unique identifier of the order.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Collection of items purchased in this order.
    /// </summary>
    public List<OrderItem> Items { get; } = new();

    /// <summary>
    /// Current status of the order.
    /// </summary>
    public string Status { get; set; } = "Pending";
}

/// <summary>
/// Child entity that lives within an <see cref="Order"/> aggregate.
/// </summary>
public sealed class OrderItem
{
    /// <summary>
    /// Primary key of the order item.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Name of the purchased product.
    /// </summary>
    public string? ProductName { get; init; }

    /// <summary>
    /// Amount of units purchased.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Price of a single unit at time of purchase.
    /// </summary>
    public decimal UnitPrice { get; init; }
}
