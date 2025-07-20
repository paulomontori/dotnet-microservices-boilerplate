using Microsoft.EntityFrameworkCore;
using PWorx.MicroserviceBoilerPlate.OrderService.Domain.Entities;

namespace PWorx.MicroserviceBoilerPlate.OrderService.Infrastructure.Data;

/// <summary>
/// Entity Framework Core <see cref="DbContext"/> representing the write database
/// for the OrderService bounded context.
/// </summary>
public sealed class OrderDbContext : DbContext
{
    /// <summary>
    /// Constructs the context with the configured options.
    /// </summary>
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    /// <summary>
    /// Configures the entity mappings using the Fluent API.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Status).IsRequired();
            entity.HasMany(o => o.Items)
                  .WithOne()
                  .HasForeignKey("OrderId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("order_items");
            entity.HasKey(i => i.Id);
            entity.Property<Guid>("OrderId");
            entity.Property(i => i.ProductName);
            entity.Property(i => i.Quantity);
            entity.Property(i => i.UnitPrice);
        });
    }
}
