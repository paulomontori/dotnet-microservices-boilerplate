namespace PWorx.MicroserviceBoilerPlate.OrderService.Domain.Entities;

public sealed class Order
{
    public Guid Id { get; init; }
    public List<OrderItem> Items { get; } = new();
    public string Status { get; set; } = "Pending";
}

public sealed class OrderItem
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string? ProductName { get; init; }
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}
