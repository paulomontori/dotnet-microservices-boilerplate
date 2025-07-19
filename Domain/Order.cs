namespace dotnet_microservices_boilerplate.Domain;

public class Order
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public IList<string> Items { get; init; } = new List<string>();
}
